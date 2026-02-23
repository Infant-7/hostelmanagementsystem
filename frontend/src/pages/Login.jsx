import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Login = () => {
  const { role: roleParam } = useParams();
  const [role, setRole] = useState(roleParam || 'student');
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const { login } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (roleParam) {
      setRole(roleParam);
    }
  }, [roleParam]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      await login(username, password, role);
      // Redirect based on role
      if (role === 'admin') {
        navigate('/admin/dashboard');
      } else if (role === 'warden') {
        navigate('/warden/dashboard');
      } else {
        navigate('/student/dashboard');
      }
    } catch (err) {
      setError(err.response?.data?.message || 'Login failed. Please check your credentials.');
    } finally {
      setLoading(false);
    }
  };

  const roleDisplay = role.charAt(0).toUpperCase() + role.slice(1);

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100 flex items-center justify-center">
      <div className="max-w-md w-full bg-white rounded-lg shadow-lg p-8">
        <h1 className="text-3xl font-bold text-center text-gray-900 mb-2">
          {roleDisplay} Login
        </h1>
        <p className="text-center text-gray-600 mb-6">
          Sign in to access your {roleDisplay.toLowerCase()} portal
        </p>

        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label htmlFor="username" className="block text-sm font-medium text-gray-700 mb-1">
              Username
            </label>
            <input
              type="text"
              id="username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
              className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-indigo-500 focus:border-indigo-500"
              placeholder="Enter your username"
            />
          </div>

          <div>
            <label htmlFor="password" className="block text-sm font-medium text-gray-700 mb-1">
              Password
            </label>
            <input
              type="password"
              id="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              className="w-full px-4 py-2 border border-gray-300 rounded-md focus:ring-indigo-500 focus:border-indigo-500"
              placeholder="Enter your password"
            />
          </div>

          <button
            type="submit"
            disabled={loading}
            className="w-full bg-indigo-600 text-white px-4 py-2 rounded-md hover:bg-indigo-700 transition disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {loading ? 'Logging in...' : 'Login'}
          </button>
        </form>

        <div className="mt-6 text-center">
          <a href="/" className="text-indigo-600 hover:text-indigo-800">
            ← Back to Home
          </a>
        </div>

        <div className="mt-6 pt-6 border-t border-gray-200">
          <p className="text-center text-sm text-gray-600 mb-2">Login as:</p>
          <div className="flex justify-center space-x-2">
            <button
              onClick={() => setRole('student')}
              className={`px-3 py-1 rounded text-sm ${
                role === 'student' ? 'bg-indigo-600 text-white' : 'bg-gray-200 text-gray-700'
              }`}
            >
              Student
            </button>
            <button
              onClick={() => setRole('admin')}
              className={`px-3 py-1 rounded text-sm ${
                role === 'admin' ? 'bg-indigo-600 text-white' : 'bg-gray-200 text-gray-700'
              }`}
            >
              Admin
            </button>
            <button
              onClick={() => setRole('warden')}
              className={`px-3 py-1 rounded text-sm ${
                role === 'warden' ? 'bg-indigo-600 text-white' : 'bg-gray-200 text-gray-700'
              }`}
            >
              Warden
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Login;



