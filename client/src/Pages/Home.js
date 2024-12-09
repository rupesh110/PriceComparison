import React, { useState } from 'react';
import { useGetProductsQuery } from '../State/ApiSlice'; // Import the query hook
import Navbar from '../Components/Navbar/Navbar';
import ProductCard from '../Components/ProductCard/ProductCard'; // Import the ProductCard component

const Home = () => {
  const [searchTerm, setSearchTerm] = useState(''); // State for input value
  const [searchQuery, setSearchQuery] = useState(''); // State for query to trigger the backend
  const { data, error, isLoading } = useGetProductsQuery(searchQuery); // Pass searchQuery to the query

  // Log the data to check its structure
  console.log(data);

  // Handle loading state
  if (isLoading) {
    return <div>Loading...</div>;
  }

  // Handle error state
  if (error) {
    return <div>Error: {error.message}</div>;
  }

  // Check if the data contains products
  if (!data || (data.Coles?.length === 0 && data.Woolworths?.length === 0)) {
    return <div>No products available.</div>;
  }

  return (
    <div>
      <Navbar />
      {/* Search Bar */}
      <div style={{ textAlign: 'center', margin: '20px 0' }}>
        <input
          type="text"
          placeholder="Search for products..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          style={{
            padding: '10px',
            width: '300px',
            borderRadius: '5px',
            border: '1px solid #ccc',
            marginRight: '10px',
          }}
        />
        <button
          onClick={() => setSearchQuery(searchTerm)} // Update the query state when button is clicked
          style={{
            padding: '10px 20px',
            backgroundColor: '#4CAF50',
            color: 'white',
            border: 'none',
            borderRadius: '5px',
            cursor: 'pointer',
          }}
        >
          Search
        </button>
      </div>

      {/* Render Coles products */}
      {data.Coles && data.Coles.length > 0 ? (
        <ProductCard products={data.Coles} title="Coles Products" />
      ) : (
        <p>No products available from Coles.</p>
      )}

      {/* Render Woolworths products */}
      {data.Woolworths && data.Woolworths.length > 0 ? (
        <ProductCard products={data.Woolworths} title="Woolworths Products" />
      ) : (
        <p>No products available from Woolworths.</p>
      )}
    </div>
  );
};

export default Home;
