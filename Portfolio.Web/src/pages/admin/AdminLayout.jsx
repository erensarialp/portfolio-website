import {
    Link,
    Outlet,
    useNavigate,
} from "react-router-dom";

import { logout } from "../../api/authApi";

function AdminLayout() {
    const navigate = useNavigate();

    function handleLogout() {
        logout();

        navigate("/admin/login");
    }

    return (
        <div>
            <aside>
                <nav>
                    <ul>
                        <li>
                            <Link to="/admin">
                                Dashboard
                            </Link>
                        </li>

                        <li>
                            <Link to="/admin/projects">
                                Projects
                            </Link>
                        </li>

                        <li>
                            <Link to="/admin/services">
                                Services
                            </Link>
                        </li>

                        <li>
                            <Link to="/admin/settings">
                                Settings
                            </Link>
                        </li>
                    </ul>
                </nav>

                <button
                    type="button"
                    onClick={handleLogout}
                >
                    Çıkış Yap
                </button>
            </aside>

            <main>
                <Outlet />
            </main>
        </div>
    );
}

export default AdminLayout;