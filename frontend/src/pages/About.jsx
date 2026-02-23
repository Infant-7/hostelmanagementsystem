import { Link } from 'react-router-dom';

const About = () => {
  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
      <nav className="bg-white shadow-md">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between h-16">
            <div className="flex items-center">
              <Link to="/" className="text-2xl font-bold text-indigo-600">Hostel Management System</Link>
            </div>
            <div className="flex items-center space-x-4">
              <Link to="/" className="text-gray-700 hover:text-indigo-600">Home</Link>
              <Link to="/about" className="text-indigo-600 font-semibold">About Us</Link>
              <Link to="/contact" className="text-gray-700 hover:text-indigo-600">Contact</Link>
            </div>
          </div>
        </div>
      </nav>

      <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-16">
        <div className="bg-white rounded-lg shadow-lg p-8">
          <h1 className="text-4xl font-bold text-gray-900 mb-6">About Us</h1>
          
          <div className="prose prose-lg max-w-none">
            <p className="text-gray-700 mb-4">
              Welcome to the Hostel Management System, a comprehensive solution designed to streamline
              and automate all aspects of hostel administration and student services.
            </p>
            
            <h2 className="text-2xl font-semibold text-gray-900 mt-8 mb-4">Our Mission</h2>
            <p className="text-gray-700 mb-4">
              Our mission is to provide an efficient, user-friendly platform that simplifies hostel
              management for administrators, wardens, and students alike. We aim to digitize and
              automate processes to save time and reduce errors.
            </p>

            <h2 className="text-2xl font-semibold text-gray-900 mt-8 mb-4">Key Features</h2>
            <ul className="list-disc list-inside text-gray-700 space-y-2 mb-4">
              <li>Comprehensive attendance tracking and management</li>
              <li>Gate pass request and approval system</li>
              <li>Room assignment and change request handling</li>
              <li>Grievance submission and resolution tracking</li>
              <li>Student and warden information management</li>
              <li>Role-based access control for secure operations</li>
            </ul>

            <h2 className="text-2xl font-semibold text-gray-900 mt-8 mb-4">Who We Serve</h2>
            <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mt-6">
              <div className="bg-indigo-50 p-4 rounded-lg">
                <h3 className="font-semibold text-indigo-900 mb-2">Students</h3>
                <p className="text-gray-700 text-sm">
                  Easy access to attendance records, request gate passes, room changes, and submit grievances.
                </p>
              </div>
              <div className="bg-green-50 p-4 rounded-lg">
                <h3 className="font-semibold text-green-900 mb-2">Wardens</h3>
                <p className="text-gray-700 text-sm">
                  Manage students, track attendance, approve requests, and handle grievances efficiently.
                </p>
              </div>
              <div className="bg-purple-50 p-4 rounded-lg">
                <h3 className="font-semibold text-purple-900 mb-2">Administrators</h3>
                <p className="text-gray-700 text-sm">
                  Full system control including managing wardens, students, and all administrative functions.
                </p>
              </div>
            </div>

            <h2 className="text-2xl font-semibold text-gray-900 mt-8 mb-4">Technology</h2>
            <p className="text-gray-700 mb-4">
              Built with modern web technologies including React for the frontend, .NET 8 Web API for
              the backend, and MySQL for reliable data storage. Our system ensures security, scalability,
              and excellent user experience.
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default About;



