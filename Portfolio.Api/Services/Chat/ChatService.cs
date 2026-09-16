using Portfolio.Api.Models.Chat;

namespace Portfolio.Api.Services.Chat
{
    public class ChatService : IChatService
    {
        private readonly IConfiguration _configuration;
        private readonly IAiChatService _aiChatService;
        private readonly IChatSessionStore _sessionStore;

        public ChatService(
            IConfiguration configuration,
            IAiChatService aiChatService,
            IChatSessionStore sessionStore)
        {
            _configuration = configuration;
            _aiChatService = aiChatService;
            _sessionStore = sessionStore;
        }

        public async Task<ChatMessageResponse> SendMessageAsync(
            ChatMessageRequest request,
            CancellationToken cancellationToken = default)
        {
            var session =
                _sessionStore.GetOrCreate(
                    request.SessionId);

            var message =
                request.Message?.Trim()
                ?? string.Empty;

            /*
             * Boş / yanlışlıkla gönderilmiş mesajlarda
             * hiçbir alan değişmez, hiçbir adım ilerlemez.
             */
            if (!ChatInputParser.IsMeaningfulMessage(message))
            {
                return CreateResponse(
                    session,
                    GetQuestionForCurrentStep(session));
            }

            /*
             * Kullanıcı bilgileri beklenenden önce
             * verdiyse bunları kaybetmiyoruz.
             */
            CaptureInformationFromAnyMessage(
                session,
                message);

            switch (session.Step)
            {
                /*
                 * STEP 0
                 * Hizmet
                 */
                case 0:
                    {
                        session.ServiceType =
                            NormalizeServiceType(
                                message);

                        session.Step = 1;

                        return await AskWithAi(
                            session,
                            message,
                            "Projen hakkında biraz daha bilgi verir misin? " +
                            "Ne tasarlamak veya üretmek istiyorsun?",
                            cancellationToken);
                    }

                /*
                 * STEP 1
                 * Proje detayı
                 */
                case 1:
                    {
                        if (!ChatInputParser
                                .IsValidProjectDetails(
                                    message))
                        {
                            return CreateResponse(
                                session,
                                "Projenin ne olduğunu biraz daha detaylı anlatabilir misin? " +
                                "Örneğin ne üretmek istediğini ve nerede kullanılacağını yazabilirsin.");
                        }

                        session.ProjectDetails =
                            message;

                        /*
                         * Tarih mesajın içinde zaten
                         * bulunuyorsa onu da yakalamış olabiliriz.
                         */
                        if (!string.IsNullOrWhiteSpace(
                                session.Deadline))
                        {
                            return await MovePastDeadline(
                                session,
                                message,
                                cancellationToken);
                        }

                        session.Step = 2;

                        return await AskWithAi(
                            session,
                            message,
                            "Bu proje için düşündüğün teslim tarihi " +
                            "veya yaklaşık zaman aralığı nedir?",
                            cancellationToken);
                    }

                /*
                 * STEP 2
                 * Deadline
                 */
                case 2:
                    {
                        var deadline =
                            ChatInputParser
                                .ExtractDeadline(
                                    message);

                        if (string.IsNullOrWhiteSpace(
                                deadline))
                        {
                            return CreateResponse(
                                session,
                                "Teslim zamanını anlayamadım. " +
                                "Örneğin \"25 Eylül\", \"gelecek hafta\" veya \"2 hafta içinde\" şeklinde yazabilir misin?");
                        }

                        session.Deadline =
                            deadline;

                        return await MovePastDeadline(
                            session,
                            message,
                            cancellationToken);
                    }

                /*
                 * STEP 3
                 * Bütçe
                 */
                case 3:
                    {
                        if (string.IsNullOrWhiteSpace(
                                session.Budget))
                        {
                            var budget =
                                ChatInputParser
                                    .ExtractBudget(
                                        message);

                            if (string.IsNullOrWhiteSpace(
                                    budget))
                            {
                                return CreateResponse(
                                    session,
                                    "Bütçe aralığını anlayamadım. " +
                                    "Örneğin \"1000-2000 TL\" şeklinde yazabilir misin?");
                            }

                            session.Budget =
                                budget;
                        }

                        /*
                         * Aynı mesajda:
                         * "İsmim Eren, bütçem 1000-2000 TL"
                         * yazılmış olabilir.
                         */
                        if (!string.IsNullOrWhiteSpace(
                                session.Name))
                        {
                            return await MovePastName(
                                session,
                                message,
                                cancellationToken);
                        }

                        session.Step = 4;

                        return await AskWithAi(
                            session,
                            message,
                            "Sana nasıl hitap edebilirim? " +
                            "Adını ve istersen soyadını paylaşabilir misin?",
                            cancellationToken);
                    }

                /*
                 * STEP 4
                 * İsim
                 */
                case 4:
                    {
                        if (string.IsNullOrWhiteSpace(
                                session.Name))
                        {
                            if (!ChatInputParser
                                    .IsValidName(
                                        message))
                            {
                                return CreateResponse(
                                    session,
                                    "İsmini anlayamadım. " +
                                    "Adını ve istersen soyadını yazar mısın?");
                            }

                            session.Name =
                                ChatInputParser
                                    .ExtractName(
                                        message);
                        }

                        return await MovePastName(
                            session,
                            message,
                            cancellationToken);
                    }

                /*
                 * STEP 5
                 * Telefon
                 */
                case 5:
                    {
                        if (string.IsNullOrWhiteSpace(
                                session.Phone))
                        {
                            var phone =
                                ChatInputParser
                                    .ExtractPhone(
                                        message);

                            if (string.IsNullOrWhiteSpace(
                                    phone))
                            {
                                return CreateResponse(
                                    session,
                                    "Telefon numaranı anlayamadım. " +
                                    "Cep telefonu numaranı yazar mısın?");
                            }

                            session.Phone =
                                phone;
                        }

                        if (!string.IsNullOrWhiteSpace(
                                session.Email))
                        {
                            return CompleteSession(
                                session);
                        }

                        session.Step = 6;

                        return await AskWithAi(
                            session,
                            message,
                            "Son olarak e-posta adresini paylaşabilir misin?",
                            cancellationToken);
                    }

                /*
                 * STEP 6
                 * E-posta
                 */
                case 6:
                    {
                        if (string.IsNullOrWhiteSpace(
                                session.Email))
                        {
                            var email =
                                ChatInputParser
                                    .ExtractEmail(
                                        message);

                            if (string.IsNullOrWhiteSpace(
                                    email))
                            {
                                return CreateResponse(
                                    session,
                                    "E-posta adresini anlayamadım. " +
                                    "Örneğin ad@ornek.com şeklinde yazabilir misin?");
                            }

                            session.Email =
                                email;
                        }

                        return CompleteSession(
                            session);
                    }

                default:
                    return CompleteSession(
                        session);
            }
        }

        private async Task<ChatMessageResponse>
            MovePastDeadline(
                ChatSession session,
                string userMessage,
                CancellationToken cancellationToken)
        {
            /*
             * Bütçe önceden verilmişse tekrar sorma.
             */
            if (string.IsNullOrWhiteSpace(
                    session.Budget))
            {
                session.Step = 3;

                return await AskWithAi(
                    session,
                    userMessage,
                    "Proje için düşündüğün yaklaşık bütçe aralığını paylaşabilir misin?",
                    cancellationToken);
            }

            /*
             * İsim de önceden verilmişse tekrar sorma.
             */
            if (string.IsNullOrWhiteSpace(
                    session.Name))
            {
                session.Step = 4;

                return await AskWithAi(
                    session,
                    userMessage,
                    "Sana nasıl hitap edebilirim? " +
                    "Adını ve istersen soyadını paylaşabilir misin?",
                    cancellationToken);
            }

            return await MovePastName(
                session,
                userMessage,
                cancellationToken);
        }

        private async Task<ChatMessageResponse>
            MovePastName(
                ChatSession session,
                string userMessage,
                CancellationToken cancellationToken)
        {
            /*
             * Telefon yoksa önce telefon iste.
             */
            if (string.IsNullOrWhiteSpace(
                    session.Phone))
            {
                session.Step = 5;

                return await AskWithAi(
                    session,
                    userMessage,
                    "Sana ulaşabilmemiz için cep telefonu numaranı paylaşabilir misin?",
                    cancellationToken);
            }

            /*
             * Mail yoksa mail iste.
             */
            if (string.IsNullOrWhiteSpace(
                    session.Email))
            {
                session.Step = 6;

                return await AskWithAi(
                    session,
                    userMessage,
                    "Son olarak e-posta adresini paylaşabilir misin?",
                    cancellationToken);
            }

            return CompleteSession(
                session);
        }

        private void CaptureInformationFromAnyMessage(
            ChatSession session,
            string message)
        {
            /*
             * Kullanıcı bir mesajın içinde
             * birden fazla bilgi verebilir.
             *
             * Örneğin:
             * "İsmim Eren, bütçem 1000-2000 TL,
             * telefonum 05..."
             */

            if (string.IsNullOrWhiteSpace(
                    session.Budget))
            {
                var budget =
                    ChatInputParser
                        .ExtractBudget(
                            message);

                if (!string.IsNullOrWhiteSpace(
                        budget))
                {
                    session.Budget =
                        budget;
                }
            }

            if (
                string.IsNullOrWhiteSpace(
                    session.Name) &&
                ChatInputParser
                    .ContainsNameDeclaration(
                        message)
            )
            {
                var name =
                    ChatInputParser
                        .ExtractName(
                            message);

                if (
                    !string.IsNullOrWhiteSpace(
                        name) &&
                    ChatInputParser
                        .IsValidName(
                            name)
                )
                {
                    session.Name =
                        name;
                }
            }

            if (string.IsNullOrWhiteSpace(
                    session.Phone))
            {
                var phone =
                    ChatInputParser
                        .ExtractPhone(
                            message);

                if (!string.IsNullOrWhiteSpace(
                        phone))
                {
                    session.Phone =
                        phone;
                }
            }

            if (string.IsNullOrWhiteSpace(
                    session.Email))
            {
                var email =
                    ChatInputParser
                        .ExtractEmail(
                            message);

                if (!string.IsNullOrWhiteSpace(
                        email))
                {
                    session.Email =
                        email;
                }
            }

            /*
             * Deadline yalnızca proje içeriği
             * aşamasından sonra otomatik yakalansın.
             *
             * Böylece proje açıklamasındaki herhangi
             * bir sayı yanlışlıkla deadline olmaz.
             */
            if (
                session.Step >= 1 &&
                string.IsNullOrWhiteSpace(
                    session.Deadline)
            )
            {
                var deadline =
                    ChatInputParser
                        .ExtractDeadline(
                            message);

                if (!string.IsNullOrWhiteSpace(
                        deadline))
                {
                    session.Deadline =
                        deadline;
                }
            }
        }

        private async Task<ChatMessageResponse>
            AskWithAi(
                ChatSession session,
                string userMessage,
                string nextQuestion,
                CancellationToken cancellationToken)
        {
            var aiResponse =
                await _aiChatService
                    .GenerateReplyAsync(
                        session,
                        userMessage,
                        nextQuestion,
                        cancellationToken);

            return CreateResponse(
                session,
                aiResponse);
        }

        private static ChatMessageResponse
            CreateResponse(
                ChatSession session,
                string message)
        {
            return new ChatMessageResponse
            {
                SessionId =
                    session.Id,

                Message =
                    message,

                Step =
                    session.Step,

                IsComplete =
                    false,

                WhatsAppUrl =
                    null
            };
        }

        private ChatMessageResponse
            CompleteSession(
                ChatSession session)
        {
            session.Step = 7;

            var summary =
                CreateWhatsAppSummary(
                    session);

            var whatsAppUrl =
                CreateWhatsAppUrl(
                    _configuration[
                        "Chat:WhatsAppNumber"],
                    summary);

            var name =
                string.IsNullOrWhiteSpace(
                    session.Name)
                    ? ""
                    : $" {session.Name}";

            return new ChatMessageResponse
            {
                SessionId =
                    session.Id,

                Message =
                    $"Teşekkürler{name}. " +
                    "Proje briefini hazırladım. " +
                    "WhatsApp üzerinden göndererek devam edebilirsin.",

                Step =
                    session.Step,

                IsComplete =
                    true,

                WhatsAppUrl =
                    whatsAppUrl
            };
        }

        private static string
            GetQuestionForCurrentStep(
                ChatSession session)
        {
            return session.Step switch
            {
                0 =>
                    "Hangi hizmetle ilgilendiğini seçebilir misin?",

                1 =>
                    "Projen hakkında biraz daha detay verebilir misin?",

                2 =>
                    "Düşündüğün teslim tarihi veya zaman aralığı nedir?",

                3 =>
                    "Yaklaşık bütçe aralığın nedir?",

                4 =>
                    "Adını ve istersen soyadını paylaşabilir misin?",

                5 =>
                    "Cep telefonu numaranı paylaşabilir misin?",

                6 =>
                    "E-posta adresini paylaşabilir misin?",

                _ =>
                    "Proje briefin tamamlandı."
            };
        }

        private static string
            CreateWhatsAppSummary(
                ChatSession session)
        {
            var phone =
                string.IsNullOrWhiteSpace(
                    session.Phone)
                    ? "-"
                    : ChatInputParser
                        .CleanPhoneForDisplay(
                            session.Phone);

            var email =
                string.IsNullOrWhiteSpace(
                    session.Email)
                    ? "-"
                    : session.Email;

            return
                "Merhaba, web sitesi üzerinden proje briefi oluşturdum.\n\n" +

                $"Seçilen Ürün: {session.ServiceType ?? "-"}\n" +
                $"Ürün İçeriği: {session.ProjectDetails ?? "-"}\n" +
                $"Bitirme Tarihi: {session.Deadline ?? "-"}\n" +
                $"Bütçe: {session.Budget ?? "-"}\n" +
                $"İsim Soyisim: {session.Name ?? "-"}\n" +
                $"İletişim Tel No: {phone}\n" +
                $"E-posta: {email}";
        }

        private static string?
            CreateWhatsAppUrl(
                string? phoneNumber,
                string summary)
        {
            if (string.IsNullOrWhiteSpace(
                    phoneNumber))
            {
                return null;
            }

            var cleanNumber =
                new string(
                    phoneNumber
                        .Where(char.IsDigit)
                        .ToArray());

            if (string.IsNullOrWhiteSpace(
                    cleanNumber))
            {
                return null;
            }

            var encodedMessage =
                Uri.EscapeDataString(
                    summary);

            return
                $"https://wa.me/{cleanNumber}?text={encodedMessage}";
        }

        private static string NormalizeServiceType(
            string serviceType)
        {
            return serviceType.Trim() switch
            {
                "Social Media" =>
                    "Sosyal Medya",

                "Graphic Design" =>
                    "Grafik Tasarım",

                "Photography" =>
                    "Fotoğraf",

                "Video / Reels" =>
                    "Video / Reels",

                "Branding" =>
                    "Marka Tasarımı",

                "Other" =>
                    "Diğer",

                _ =>
                    serviceType.Trim()
            };
        }
    }
}