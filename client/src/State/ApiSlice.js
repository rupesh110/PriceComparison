import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const baseUrl = process.env.REACT_APP_API_BASE_URL;
const apiKey = process.env.REACT_APP_API_KEY;

console.log(baseUrl);

export const apiSlice = createApi({
  reducerPath: 'api',
  baseQuery: fetchBaseQuery({ baseUrl }),
  endpoints: (builder) => ({
    getProducts: builder.query({
      // Dynamically add the API key and query parameter
      query: (searchQuery) => `Products?code=${apiKey}&query=${searchQuery || 'Latte'}`,
    }),
  }),
});

export const { useGetProductsQuery } = apiSlice;
