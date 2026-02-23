import { Link } from 'react-router-dom';

const Home = () => {
  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
      <nav className="bg-white shadow-md">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between h-16">
            <div className="flex items-center">
              <h1 className="text-2xl font-bold text-indigo-600">Hostel Management System</h1>
            </div>
            <div className="flex items-center space-x-4">
              <Link to="/" className="text-gray-700 hover:text-indigo-600">Home</Link>
              <Link to="/about" className="text-gray-700 hover:text-indigo-600">About Us</Link>
              <Link to="/contact" className="text-gray-700 hover:text-indigo-600">Contact</Link>
              <Link to="/login/student" className="bg-indigo-600 text-white px-4 py-2 rounded-md hover:bg-indigo-700">
                Student Login
              </Link>
              <Link to="/login/admin" className="bg-indigo-600 text-white px-4 py-2 rounded-md hover:bg-indigo-700">
                Admin Login
              </Link>
              <Link to="/login/warden" className="bg-indigo-600 text-white px-4 py-2 rounded-md hover:bg-indigo-700">
                Warden Login
              </Link>
            </div>
          </div>
        </div>
      </nav>

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-16">
        <div className="text-center">
          <h1 className="text-5xl font-bold text-gray-900 mb-6">
            Welcome to Hostel Management System
          </h1>
          <p className="text-xl text-gray-600 mb-8">
            Streamline your hostel operations with our comprehensive management solution
          </p>
          <div className="flex justify-center space-x-4">
            <Link
              to="/login/student"
              className="bg-indigo-600 text-white px-8 py-3 rounded-lg text-lg font-semibold hover:bg-indigo-700 transition"
            >
              Student Portal
            </Link>
            <Link
              to="/login/admin"
              className="bg-gray-600 text-white px-8 py-3 rounded-lg text-lg font-semibold hover:bg-gray-700 transition"
            >
              Admin Portal
            </Link>
            <Link
              to="/login/warden"
              className="bg-green-600 text-white px-8 py-3 rounded-lg text-lg font-semibold hover:bg-green-700 transition"
            >
              Warden Portal
            </Link>
          </div>
        </div>

        <div className="mt-20 grid grid-cols-1 md:grid-cols-3 gap-8">
          <div className="bg-white p-6 rounded-lg shadow-lg">
            <div className="text-4xl mb-4">📊</div>
            <h3 className="text-xl font-semibold mb-2">Attendance Management</h3>
            <p className="text-gray-600">Track and manage student attendance efficiently</p>
          </div>
          <div className="bg-white p-6 rounded-lg shadow-lg">
            <div className="text-4xl mb-4">🚪</div>
            <h3 className="text-xl font-semibold mb-2">Gate Pass System</h3>
            <p className="text-gray-600">Handle gate pass requests and approvals seamlessly</p>
          </div>
          <div className="bg-white p-6 rounded-lg shadow-lg">
            <div className="text-4xl mb-4">🏠</div>
            <h3 className="text-xl font-semibold mb-2">Room Management</h3>
            <p className="text-gray-600">Manage room assignments and change requests</p>
          </div>
          <div className="bg-white p-6 rounded-lg shadow-lg">
            <div className="text-4xl mb-4">💬</div>
            <h3 className="text-xl font-semibold mb-2">Grievance System</h3>
            <p className="text-gray-600">Submit and track grievances easily</p>
          </div>
          <div className="bg-white p-6 rounded-lg shadow-lg">
            <div className="text-4xl mb-4">👥</div>
            <h3 className="text-xl font-semibold mb-2">Student Management</h3>
            <p className="text-gray-600">Comprehensive student information management</p>
          </div>
          <div className="bg-white p-6 rounded-lg shadow-lg">
            <div className="text-4xl mb-4">🔐</div>
            <h3 className="text-xl font-semibold mb-2">Secure Access</h3>
            <p className="text-gray-600">Role-based access control for all users</p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Home;



