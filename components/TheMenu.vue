<template>
    <UHorizontalNavigation
      :links="links"
      class="border-b border-gray-200 dark:border-gray-800"
    />
  </template>
  
  <script setup lang="ts">
  const route = useRoute();
  
  const links = [
    {
      label: "Profiil",
      to: "/profiles",
    },
    {
      label: "Loo Arve",
      to: "/create-invoice",
    },
    {
      label: "Avaleht",
      to: "/",
    },
    
  ];
  </script>