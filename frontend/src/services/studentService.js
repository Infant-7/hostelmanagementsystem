import api from './api';

export const studentService = {
  getAll: async () => {
    const response = await api.get('/students');
    return response.data;
  },

  getById: async (id) => {
    const response = await api.get(`/students/${id}`);
    return response.data;
  },

  create: async (data) => {
    const response = await api.post('/students', data);
    return response.data;
  },

  update: async (id, data) => {
    await api.put(`/students/${id}`, data);
  },

  delete: async (id) => {
    await api.delete(`/students/${id}`);
  },
};



