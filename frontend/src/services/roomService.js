import api from './api';

export const roomService = {
  getAll: async () => {
    const response = await api.get('/rooms');
    return response.data;
  },

  getById: async (id) => {
    const response = await api.get(`/rooms/${id}`);
    return response.data;
  },

  create: async (data) => {
    const response = await api.post('/rooms', data);
    return response.data;
  },

  update: async (id, data) => {
    await api.put(`/rooms/${id}`, data);
  },

  delete: async (id) => {
    await api.delete(`/rooms/${id}`);
  },
};



