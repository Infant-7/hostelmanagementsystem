import api from './api';

export const wardenService = {
  getAll: async () => {
    const response = await api.get('/wardens');
    return response.data;
  },

  getById: async (id) => {
    const response = await api.get(`/wardens/${id}`);
    return response.data;
  },

  create: async (data) => {
    const response = await api.post('/wardens', data);
    return response.data;
  },

  update: async (id, data) => {
    await api.put(`/wardens/${id}`, data);
  },

  delete: async (id) => {
    await api.delete(`/wardens/${id}`);
  },
};



