import { useCallback } from 'react';
import { useAuth } from '../context/AuthContext';

const API_BASE_URL = 'http://localhost:5100/api/messages';

export const useMessagingAPI = () => {
  const { user, logout } = useAuth();
  const token = user?.token || user?.accessToken || null;

  const getHeaders = () => ({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  });

  const parseJsonSafe = async (response) => {
    const text = await response.text();
    if (!text) return null;

    try {
      return JSON.parse(text);
    } catch {
      return null;
    }
  };

  // Lấy danh sách người dùng có thể nhắn tin (Admin, GiáoViên, PhụHuynh)
  const getUsers = useCallback(async () => {
    try {
      if (!token) return [];

      const response = await fetch(`${API_BASE_URL}/users`, {
        method: 'GET',
        headers: getHeaders()
      });

      if (!response.ok) {
        if (response.status === 401) {
          console.warn('Chưa đăng nhập hoặc token hết hạn khi lấy danh sách người dùng');
          logout?.();
          return [];
        }
        const errorData = await parseJsonSafe(response);
        console.error('Lỗi lấy danh sách người dùng:', errorData?.message || response.statusText);
        return [];
      }

      const data = await parseJsonSafe(response);
      if (data.success) {
        return (data.data || []).map((u) => ({
          id: u?.id || u?.userId,
          name: u?.name || u?.fullName,
          role: u?.role,
          avatarUrl: u?.avatarUrl || null
        }));
      } else {
        console.error('Lỗi lấy danh sách người dùng:', data.message);
        return [];
      }
    } catch (error) {
      console.error('Lỗi kết nối lấy danh sách người dùng:', error);
      return [];
    }
  }, [token]);

  // Lấy danh sách cuộc trò chuyện
  const getConversations = useCallback(async () => {
    try {
      if (!token) return [];

      const response = await fetch(`${API_BASE_URL}/conversations`, {
        method: 'GET',
        headers: getHeaders()
      });

      if (!response.ok) {
        if (response.status === 401) {
          console.warn('Chưa đăng nhập hoặc token hết hạn khi lấy cuộc trò chuyện');
          logout?.();
          return [];
        }
        const errorData = await parseJsonSafe(response);
        console.error('Lỗi lấy danh sách cuộc trò chuyện:', errorData?.message || response.statusText);
        return [];
      }

      const data = await parseJsonSafe(response);
      if (data.success) {
        return data.data || [];
      } else {
        console.error('Lỗi lấy danh sách cuộc trò chuyện:', data.message);
        return [];
      }
    } catch (error) {
      console.error('Lỗi kết nối lấy cuộc trò chuyện:', error);
      return [];
    }
  }, [token]);

  // Lấy tin nhắn của một cuộc trò chuyện
  const getMessages = useCallback(async (conversationId, take = 50) => {
    try {
      if (!token) return [];

      const response = await fetch(`${API_BASE_URL}/conversations/${conversationId}/messages?take=${take}`, {
        method: 'GET',
        headers: getHeaders()
      });

      if (!response.ok) {
        if (response.status === 401) {
          console.warn('Chưa đăng nhập hoặc token hết hạn khi lấy tin nhắn');
          logout?.();
          return [];
        }
        const errorData = await parseJsonSafe(response);
        console.error('Lỗi lấy tin nhắn:', errorData?.message || response.statusText);
        return [];
      }

      const data = await parseJsonSafe(response);
      if (data.success) {
        return data.data || [];
      } else {
        console.error('Lỗi lấy tin nhắn:', data.message);
        return [];
      }
    } catch (error) {
      console.error('Lỗi kết nối lấy tin nhắn:', error);
      return [];
    }
  }, [token]);

  // Gửi tin nhắn
  const sendMessage = useCallback(async (conversationId, content, attachmentUrls = []) => {
    try {
      if (!token) return null;

      const response = await fetch(`${API_BASE_URL}/conversations/${conversationId}/messages`, {
        method: 'POST',
        headers: getHeaders(),
        body: JSON.stringify({
          content,
          attachmentUrls: attachmentUrls.length > 0 ? attachmentUrls : undefined
        })
      });

      if (!response.ok) {
        if (response.status === 401) {
          console.warn('Chưa đăng nhập hoặc token hết hạn khi gửi tin nhắn');
          logout?.();
          return null;
        }
        const errorData = await parseJsonSafe(response);
        console.error('Lỗi gửi tin nhắn:', errorData?.message || response.statusText);
        return null;
      }

      const data = await parseJsonSafe(response);
      if (data.success) {
        return data.data;
      } else {
        console.error('Lỗi gửi tin nhắn:', data.message);
        return null;
      }
    } catch (error) {
      console.error('Lỗi kết nối gửi tin nhắn:', error);
      return null;
    }
  }, [token]);

  // Tạo cuộc trò chuyện 1-1
  const createDirectConversation = useCallback(async (recipientId, initialMessage = null) => {
    try {
      if (!token) return null;

      const response = await fetch(`${API_BASE_URL}/direct`, {
        method: 'POST',
        headers: getHeaders(),
        body: JSON.stringify({
          recipientUserId: recipientId,
          initialMessage
        })
      });

      if (!response.ok) {
        if (response.status === 401) {
          console.warn('Chưa đăng nhập hoặc token hết hạn khi tạo cuộc trò chuyện');
          logout?.();
          return null;
        }
        const errorData = await parseJsonSafe(response);
        console.error('Lỗi tạo cuộc trò chuyện:', errorData?.message || response.statusText);
        return null;
      }

      const data = await parseJsonSafe(response);
      if (data.success) {
        return data.data;
      } else {
        console.error('Lỗi tạo cuộc trò chuyện:', data.message);
        return null;
      }
    } catch (error) {
      console.error('Lỗi kết nối tạo cuộc trò chuyện:', error);
      return null;
    }
  }, [token]);

  // Tạo nhóm chat
  const createGroupConversation = useCallback(async (title, memberIds, initialMessage = null) => {
    try {
      if (!token) return null;

      const response = await fetch(`${API_BASE_URL}/groups`, {
        method: 'POST',
        headers: getHeaders(),
        body: JSON.stringify({
          title,
          memberUserIds: memberIds,
          initialMessage
        })
      });

      if (!response.ok) {
        if (response.status === 401) {
          console.warn('Chưa đăng nhập hoặc token hết hạn khi tạo nhóm chat');
          logout?.();
          return null;
        }
        const errorData = await parseJsonSafe(response);
        console.error('Lỗi tạo nhóm chat:', errorData?.message || response.statusText);
        return null;
      }

      const data = await parseJsonSafe(response);
      if (data.success) {
        return data.data;
      } else {
        console.error('Lỗi tạo nhóm chat:', data.message);
        return null;
      }
    } catch (error) {
      console.error('Lỗi kết nối tạo nhóm chat:', error);
      return null;
    }
  }, [token]);

  // Đánh dấu cuộc trò chuyện đã đọc
  const markAsRead = useCallback(async (conversationId) => {
    try {
      if (!token) return false;

      const response = await fetch(`${API_BASE_URL}/conversations/${conversationId}/read`, {
        method: 'POST',
        headers: getHeaders()
      });

      if (!response.ok) {
        if (response.status === 401) {
          console.warn('Chưa đăng nhập hoặc token hết hạn khi đánh dấu đã đọc');
          logout?.();
          return false;
        }
        return false;
      }

      const data = await parseJsonSafe(response);
      return data.success;
    } catch (error) {
      console.error('Lỗi kết nối đánh dấu đã đọc:', error);
      return false;
    }
  }, [token]);

  // Đăng ký device token cho push notifications
  const registerDeviceToken = useCallback(async (deviceToken) => {
    try {
      if (!token) return false;

      const response = await fetch(`${API_BASE_URL}/device-token`, {
        method: 'POST',
        headers: getHeaders(),
        body: JSON.stringify({
          deviceToken
        })
      });

      if (!response.ok) {
        if (response.status === 401) {
          console.warn('Chưa đăng nhập hoặc token hết hạn khi đăng ký device token');
          logout?.();
          return false;
        }
        return false;
      }

      const data = await parseJsonSafe(response);
      return data.success;
    } catch (error) {
      console.error('Lỗi kết nối đăng ký device token:', error);
      return false;
    }
  }, [token]);

  return {
    getUsers,
    getConversations,
    getMessages,
    sendMessage,
    createDirectConversation,
    createGroupConversation,
    markAsRead,
    registerDeviceToken
  };
};
