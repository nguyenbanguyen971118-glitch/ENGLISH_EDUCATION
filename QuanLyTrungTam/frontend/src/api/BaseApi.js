class BaseApi {
    // - chuc nang: Khoi tao BaseApi voi URL goc cua backend.
    // - nmkhue -29/2/2026
    constructor(baseURL) {
        this.baseURL = baseURL;
        this.refreshInProgress = false;
        this.refreshTokenProvider = null;
    }

    // - chuc nang: Dang ky callback de lay refresh token tu AuthContext.
    // - nmkhue -31/3/2026
    setRefreshTokenProvider(callback) {
        this.refreshTokenProvider = callback;
    }

    // - chuc nang: Lay access token da luu trong localStorage.
    // - nmkhue -29/2/2026
    getAuthToken() {
        const userData = localStorage.getItem('user');
        if (!userData) {
            return null;
        }

        try {
            const parsed = JSON.parse(userData);
            return parsed?.token || null;
        } catch {
            return null;
        }
    }

    // - chuc nang: Lay refresh token va sessionId da luu trong localStorage.
    // - nmkhue -31/3/2026
    getRefreshTokenData() {
        const userData = localStorage.getItem('user');
        if (!userData) {
            return { refreshToken: null, sessionId: null };
        }

        try {
            const parsed = JSON.parse(userData);
            return { 
                refreshToken: parsed?.refreshToken || null,
                sessionId: parsed?.sessionId || null
            };
        } catch {
            return { refreshToken: null, sessionId: null };
        }
    }

    // - chuc nang: Chuyen endpoint tuong doi thanh URL day du.
    // - nmkhue -29/2/2026
    buildUrl(path) {
        if (!path) {
            return this.baseURL;
        }

        if (path.startsWith('http://') || path.startsWith('https://')) {
            return path;
        }

        const cleanPath = path.startsWith('/') ? path.slice(1) : path;
        return `${this.baseURL}/${cleanPath}`;
    }

    // - chuc nang: Gui request tong quat, gan token, parse JSON va chuan hoa loi. Ho tro auto-refresh token khi het han.
    // - nmkhue -31/3/2026
    async request(path, options = {}) {
        const token = this.getAuthToken();
        const headers = {
            'Content-Type': 'application/json',
            ...(options.headers || {}),
        };

        if (token) {
            headers.Authorization = `Bearer ${token}`;
        }

        const response = await fetch(this.buildUrl(path), {
            ...options,
            headers,
        });

        const contentType = response.headers.get('content-type') || '';
        const isJson = contentType.includes('application/json');
        const payload = isJson ? await response.json() : null;

        // Neu gap 401 chi thu refresh khi co access token.
        // Request public/anonymous (nhu trang login) khong duoc tu dong redirect loop.
        if (response.status === 401 && token && !this.refreshInProgress && this.refreshTokenProvider) {
            this.refreshInProgress = true;
            try {
                const refreshResult = await this.refreshTokenProvider();
                this.refreshInProgress = false;

                if (refreshResult?.success) {
                    // Retry request cu voi token moi
                    return this.request(path, options);
                } else {
                    // Refresh that bai, logout
                    localStorage.removeItem('user');
                    window.location.href = '/login';
                    return {
                        success: false,
                        message: 'Phiên đã hết hạn, vui lòng đăng nhập lại.',
                        status: 401,
                        data: null,
                    };
                }
            } catch (error) {
                this.refreshInProgress = false;
                localStorage.removeItem('user');
                window.location.href = '/login';
                return {
                    success: false,
                    message: 'Phiên đã hết hạn, vui lòng đăng nhập lại.',
                    status: 401,
                    data: null,
                };
            }
        }

        if (!response.ok) {
            if (response.status === 401 || response.status === 403) {
                localStorage.removeItem('user');
            }

            const validationMessage = Array.isArray(payload?.errors) && payload.errors.length > 0
                ? payload.errors.join(' ')
                : Array.isArray(payload?.data?.errors) && payload.data.errors.length > 0
                    ? payload.data.errors.join(' ')
                    : '';

            return {
                success: false,
                message: payload?.message || payload?.error || validationMessage || `Request failed with status ${response.status}`,
                status: response.status,
                data: payload?.data,
                raw: payload,
            };
        }

        return payload;
    }

    // - chuc nang: Shortcut gui HTTP GET.
    // - nmkhue -29/2/2026
    get(path, options = {}) {
        return this.request(path, { ...options, method: 'GET' });
    }

    // - chuc nang: Shortcut gui HTTP POST.
    // - nmkhue -29/2/2026
    post(path, body, options = {}) {
        return this.request(path, {
            ...options,
            method: 'POST',
            body: body ? JSON.stringify(body) : undefined,
        });
    }

    // - chuc nang: Shortcut gui HTTP PUT.
    // - nmkhue -29/2/2026
    put(path, body, options = {}) {
        return this.request(path, {
            ...options,
            method: 'PUT',
            body: body ? JSON.stringify(body) : undefined,
        });
    }

    // - chuc nang: Shortcut gui HTTP DELETE.
    // - nmkhue -29/2/2026
    delete(path, options = {}) {
        return this.request(path, { ...options, method: 'DELETE' });
    }
}


const DEFAULT_BACKEND_BASE_URL = import.meta.env.DEV ? '/api' : 'http://localhost:5050';


const normalizeApiBaseUrl = (baseUrl = DEFAULT_BACKEND_BASE_URL) => {
    const trimmedBaseUrl = baseUrl.trim().replace(/\/+$/, '');

    if (trimmedBaseUrl === '') {
        return '/api';
    }

    if (/\/api$/i.test(trimmedBaseUrl)) {
        return trimmedBaseUrl;
    }

    return `${trimmedBaseUrl}/api`;
};

const getApiBaseUrl = () => {
    const envBaseUrl = import.meta.env.VITE_API_BASE_URL?.trim();

    // Trong môi trường dev, nếu backend được cấu hình localhost thì dùng proxy /api để tránh lỗi CORS
    if (import.meta.env.DEV && (!envBaseUrl || envBaseUrl.includes('localhost') || envBaseUrl.includes('127.0.0.1'))) {
        return normalizeApiBaseUrl('/api');
    }

    return normalizeApiBaseUrl(envBaseUrl || DEFAULT_BACKEND_BASE_URL);
};

const getSignalRBaseUrl = () => getApiBaseUrl().replace(/\/api$/i, '');

const apiClient = new BaseApi(getApiBaseUrl());

export { BaseApi, getApiBaseUrl, getSignalRBaseUrl, normalizeApiBaseUrl };
export default apiClient;