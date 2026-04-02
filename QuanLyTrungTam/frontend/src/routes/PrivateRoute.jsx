import { Navigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const PrivateRoute = ({
    children,
    allowedRoles = [],
    requiredPermissions = [],
    requireAllPermissions = true
}) => {
    const { user } = useAuth();

    if (!user) {
        return <Navigate to="/login" replace />;
    }

    // Luon check role neu route co khai bao allowedRoles.
    if (allowedRoles.length > 0 && !allowedRoles.includes(user.role)) {
        return <Navigate to="/unauthorized" replace />;
    }

    // Neu route co yeu cau permission, can vuot qua check permission.
    if (requiredPermissions.length > 0) {
        const userPermissions = Array.isArray(user.permissionCodes) ? user.permissionCodes : [];

        const hasRequiredPermissions = requireAllPermissions
            ? requiredPermissions.every((permission) => userPermissions.includes(permission))
            : requiredPermissions.some((permission) => userPermissions.includes(permission));

        if (!hasRequiredPermissions) {
            return <Navigate to="/unauthorized" replace />;
        }
    }

    return children;
};

export default PrivateRoute;