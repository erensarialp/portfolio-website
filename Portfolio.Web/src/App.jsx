import {
    BrowserRouter,
    Navigate,
    Route,
    Routes,
} from "react-router-dom";

import HomePage from "./pages/HomePage";

import AdminLoginPage from "./pages/admin/AdminLoginPage";
import AdminLayout from "./pages/admin/AdminLayout";
import DashboardPage from "./pages/admin/DashboardPage";
import ProjectsPage from "./pages/admin/ProjectsPage";
import ProjectEditPage from "./pages/admin/ProjectEditPage";
import ServicesPage from "./pages/admin/ServicesPage";
import SettingsPage from "./pages/admin/SettingsPage";

import { isAuthenticated } from "./api/authApi";

function ProtectedRoute({ children }) {
    if (!isAuthenticated()) {
        return (
            <Navigate
                to="/admin/login"
                replace
            />
        );
    }

    return children;
}

function App() {
    return (
        <BrowserRouter>
            <Routes>

                {/* PUBLIC */}
                <Route
                    path="/"
                    element={<HomePage />}
                />

                {/* ADMIN LOGIN */}
                <Route
                    path="/admin/login"
                    element={<AdminLoginPage />}
                />

                {/* ADMIN */}
                <Route
                    path="/admin"
                    element={
                        <ProtectedRoute>
                            <AdminLayout />
                        </ProtectedRoute>
                    }
                >
                    <Route
                        index
                        element={<DashboardPage />}
                    />

                    <Route
                        path="projects"
                        element={<ProjectsPage />}
                    />

                    <Route
                        path="projects/new"
                        element={<ProjectEditPage />}
                    />

                    <Route
                        path="projects/:id"
                        element={<ProjectEditPage />}
                    />

                    <Route
                        path="services"
                        element={<ServicesPage />}
                    />

                    <Route
                        path="settings"
                        element={<SettingsPage />}
                    />
                </Route>

                {/* BİLİNMEYEN ROUTE */}
                <Route
                    path="*"
                    element={
                        <Navigate
                            to="/"
                            replace
                        />
                    }
                />

            </Routes>
        </BrowserRouter>
    );
}

export default App;