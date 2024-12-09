import React from 'react';
import './ProductCard.css'; // Import CSS for styling

const ProductCard = ({ products, title }) => {
  return (
    <div>
      <h2>{title}</h2>
      <div className="products-container">
        {products.map((product, index) => (
          <div key={index} className="product-card">
            <h3 className="product-name">{product.ProductName}</h3>
            <p className="product-brand">Brand: {product.ProductBrand}</p>
            <p className="product-price">Price: {product.CurrentPrice}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default ProductCard;
