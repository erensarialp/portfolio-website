import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login, saveToken } from "../../api/authApi";

function AdminLoginPage() {
    const navigate = useNavigate();

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    async function handleSubmit(event) {
        event.preventDefault();

        setError("");
        setLoading(true);

        try {
            const response = await login(username, password);

            saveToken(response.token);

            navigate("/admin");
        } catch (err) {
            console.error(err);

            if (err.response?.status === 401) {
                setError("Kullanıcı adı veya şifre hatalı.");
            } else {
                setError("Giriş yapılırken bir hata oluştu.");
            }
        } finally {
            setLoading(false);
        }
    }

    return (
        <main>
            <h1>Admin Login</h1>

            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="username">
                        Kullanıcı adı
                    </label>

                    <input
                        id="username"
                        type="text"
                        value={username}
                        onChange={(event) =>
                            setUsername(event.target.value)
                        }
                        required
                    />
                </div>

                <div>
                    <label htmlFor="password">
                        Şifre
                    </label>

                    <input
                        id="password"
                        type="password"
                        value={password}
                        onChange={(event) =>
                            setPassword(event.target.value)
                        }
                        required
                    />
                </div>

                {error && (
                    <p>
                        {error}
                    </p>
                )}

                <button
                    type="submit"
                    disabled={loading}
                >
                    {loading
                        ? "Giriş yapılıyor..."
                        : "Giriş Yap"}
                </button>
            </form>
        </main>
    );
}

export default AdminLoginPage;