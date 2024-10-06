<template>
    <UHorizontalNavigation
      :links="links"
      class="border-b border-gray-200 dark:border-gray-800"
    />
  </template>
  
  <script setup lang="ts">
  import { useRoute } from 'vue-router';

  const route = useRoute();
  
  const links = [
  {
      label: "Avaleht",
      to: "/",
    },
    {
      label: "Profiil",
      to: "/profiles",
    },
    {
      label: "Tooted",
      to: "/products",
    },
    {
      label: "Loo Arve",
      to: "/create-invoice",
    },
    
  ];
  </script>