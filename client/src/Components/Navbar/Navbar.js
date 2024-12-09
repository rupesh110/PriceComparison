import React from 'react';
import './Navbar.css'; // Import the CSS file

const Navbar = () => {
  return (
    <div className="navbar">
      <div className="logo">PriceCompare</div>
      <div className="menu">
        <span>Home</span>
        <span>Products</span>
        <span>About</span>
        <span>Contact</span>
      </div>
    </div>
  );
};

export default Navbar;
