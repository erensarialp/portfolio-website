using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Portfolio.Api.Services.Chat
{
    public class NvidiaChatService : IAiChatService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public NvidiaChatService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GenerateReplyAsync(
            ChatSession session,
            string userMessage,
            string nextQuestion,
            CancellationToken cancellationToken = default)
        {
            var apiKey =
                _configuration["Nvidia:ApiKey"];

            var model =
                _configuration["Nvidia:Model"]
                ?? "nvidia/nemotron-3.5-lightning-30b-a3b";

            var baseUrl =
                _configuration["Nvidia:BaseUrl"]
                ?? "https://integrate.api.nvidia.com/v1";

            /*
             * API key bulunamazsa chatbot tamamen bozulmasın.
             * AI yerine kontrollü nextQuestion metni kullanılır.
             */
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return nextQuestion;
            }

            var systemPrompt = """
                Sen bir grafik tasarımcı ve fotoğrafçının web sitesinde çalışan
                yaratıcı proje asistanısın.

                Amacın potansiyel müşteriden kısa ve kullanışlı bir proje briefi toplamaktır.

                Konuşma dili her zaman Türkçe olmalıdır.

                ÇOK ÖNEMLİ KURALLAR:

                - Sıcak ama profesyonel konuş.
                - Cevabın maksimum 2 kısa cümle olsun.
                - Kullanıcının son mesajına kısa ve doğal biçimde karşılık verebilirsin.
                - Sana verilen NEXT QUESTION yalnızca hangi bilgiyi istemen gerektiğini belirler.
                - NEXT QUESTION dışında başka bir bilgi isteme.
                - Aynı mesaj içinde birden fazla soru sorma.
                - Kullanıcının daha önce verdiği bilgiyi tekrar isteme.
                - İsim zaten biliniyorsa tekrar isim sorma.
                - Bütçe zaten biliniyorsa tekrar bütçe sorma.
                - Telefon veya e-posta zaten biliniyorsa aynı bilgiyi tekrar isteme.
                - Kullanıcının mesajını veri olarak olduğu gibi tekrar etme.
                - Yeni bilgi uydurma.
                - Projenin kabul edildiğini söyleme.
                - Tasarımcı adına fiyat verme.
                - Fiyat garantisi verme.
                - Teslim tarihi garantisi verme.
                - "25 Eylül'e yetiştiririm", "o tarihe kadar hazır olur",
                  "projeyi zamanında tamamlarız" gibi ifadeler ASLA kullanma.
                - Kullanıcının istediği tarihin mümkün olduğunu doğrulama.
                - Gereksiz emoji kullanma.
                - Markdown kullanma.
                - JSON döndürme.
                - Düşünme sürecini yazma.
                - Analiz veya reasoning adımlarını yazma.
                - "Thinking process", "analysis", "step 1", "step 2",
                  "reasoning" veya benzeri dahili açıklamalar üretme.
                - Yalnızca son kullanıcıya gösterilecek cevabı üret.
                """;

            var userPrompt = $"""
                Şu ana kadar sistem tarafından toplanmış proje briefi:

                Hizmet:
                {session.ServiceType ?? "-"}

                Proje detayı:
                {session.ProjectDetails ?? "-"}

                Teslim zamanı:
                {session.Deadline ?? "-"}

                Bütçe:
                {session.Budget ?? "-"}

                İsim:
                {session.Name ?? "-"}

                Telefon:
                {session.Phone ?? "-"}

                E-posta:
                {session.Email ?? "-"}

                Kullanıcının son mesajı:
                {userMessage}

                NEXT QUESTION:
                {nextQuestion}

                Görevin:

                1. Kullanıcının son mesajına gerekirse çok kısa ve doğal bir karşılık ver.
                2. Ardından yalnızca NEXT QUESTION ile belirtilen bilgiyi sor.
                3. Yukarıdaki briefte zaten dolu olan hiçbir bilgiyi tekrar isteme.
                4. Başka bir bilgiye geçme.
                5. Tek soru sor.
                6. Yalnızca kullanıcıya gösterilecek Türkçe cevabı üret.
                """;

            using var request =
                new HttpRequestMessage(
                    HttpMethod.Post,
                    $"{baseUrl.TrimEnd('/')}/chat/completions");

            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    apiKey);

            request.Content =
                JsonContent.Create(
                    new
                    {
                        model,

                        messages = new object[]
                        {
                            new
                            {
                                role = "system",
                                content = systemPrompt
                            },

                            new
                            {
                                role = "user",
                                content = userPrompt
                            }
                        },

                        /*
                         * Daha düşük temperature:
                         * Chatbotun daha tutarlı davranmasını sağlar.
                         */
                        temperature = 0.2,

                        top_p = 0.8,

                        /*
                         * Kısa müşteri cevapları istediğimiz için
                         * yüksek token değerine ihtiyacımız yok.
                         */
                        max_tokens = 80,

                        stream = false,

                        /*
                         * Nemotron reasoning çıktısını
                         * kullanıcıya göstermesin.
                         */
                        chat_template_kwargs = new
                        {
                            enable_thinking = false
                        }
                    });

            try
            {
                using var response =
                    await _httpClient.SendAsync(
                        request,
                        cancellationToken);

                /*
                 * NVIDIA başarısız dönerse
                 * kullanıcı hata görmesin.
                 */
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent =
                        await response.Content
                            .ReadAsStringAsync(
                                cancellationToken);

                    Console.WriteLine(
                        $"NVIDIA API error: " +
                        $"{response.StatusCode} " +
                        errorContent);

                    return nextQuestion;
                }

                var responseJson =
                    await response.Content
                        .ReadAsStringAsync(
                            cancellationToken);

                var generatedText =
                    ExtractAssistantMessage(
                        responseJson);

                if (string.IsNullOrWhiteSpace(
                        generatedText))
                {
                    return nextQuestion;
                }

                return CleanResponse(
                    generatedText,
                    nextQuestion);
            }
            catch (TaskCanceledException ex)
            {
                /*
                 * NVIDIA timeout olursa sohbet
                 * bizim kontrollü sorumuzla devam eder.
                 */
                Console.WriteLine(
                    $"NVIDIA API timeout: {ex.Message}");

                return nextQuestion;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"NVIDIA API request error: {ex.Message}");

                return nextQuestion;
            }
        }

        private static string? ExtractAssistantMessage(
            string responseJson)
        {
            try
            {
                using var document =
                    JsonDocument.Parse(
                        responseJson);

                var root =
                    document.RootElement;

                if (
                    !root.TryGetProperty(
                        "choices",
                        out var choices) ||
                    choices.ValueKind !=
                        JsonValueKind.Array ||
                    choices.GetArrayLength() == 0
                )
                {
                    return null;
                }

                var firstChoice =
                    choices[0];

                if (
                    !firstChoice.TryGetProperty(
                        "message",
                        out var message)
                )
                {
                    return null;
                }

                if (
                    !message.TryGetProperty(
                        "content",
                        out var content)
                )
                {
                    return null;
                }

                if (
                    content.ValueKind !=
                        JsonValueKind.String
                )
                {
                    return null;
                }

                return content.GetString();
            }
            catch (JsonException ex)
            {
                Console.WriteLine(
                    $"NVIDIA response JSON parse error: {ex.Message}");

                return null;
            }
        }

        private static string CleanResponse(
            string generatedText,
            string fallback)
        {
            var text =
                generatedText.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                return fallback;
            }

            /*
             * Modelin reasoning metnini yanlışlıkla
             * kullanıcıya döndürmesine karşı ikinci koruma.
             */
            var suspiciousPatterns =
                new[]
                {
                    "thinking process",
                    "here's a thinking process",
                    "analyze user input",
                    "analysis:",
                    "reasoning:",
                    "chain of thought",
                    "step 1",
                    "step 2",
                    "system prompt",
                    "next question:"
                };

            foreach (var pattern in suspiciousPatterns)
            {
                if (
                    text.Contains(
                        pattern,
                        StringComparison.OrdinalIgnoreCase)
                )
                {
                    return fallback;
                }
            }

            /*
             * Teslim garantisi gibi istemediğimiz
             * riskli ifadeler oluşursa AI cevabını
             * kullanmak yerine güvenli soruya dön.
             */
            var forbiddenPromises =
                new[]
                {
                    "yetiştiririm",
                    "yetiştirebiliriz",
                    "teslim ederim",
                    "teslim ederiz",
                    "hazır olur",
                    "tamamlarız",
                    "garanti ederim",
                    "garanti ediyoruz",
                    "kesin yetişir",
                    "kesinlikle yetişir"
                };

            foreach (var phrase in forbiddenPromises)
            {
                if (
                    text.Contains(
                        phrase,
                        StringComparison.OrdinalIgnoreCase)
                )
                {
                    return fallback;
                }
            }

            return text;
        }
    }
}