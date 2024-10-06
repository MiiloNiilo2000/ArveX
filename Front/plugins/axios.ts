import axios from 'axios';
import { defineNuxtPlugin } from '#app';

export default defineNuxtPlugin(() => {
  const api = axios.create({
    baseURL: 'http://localhost:5176', 
  });

  return {
    provide: {
      axios: api,
    },
  };
});