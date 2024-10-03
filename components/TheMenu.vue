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
      label: "Avaleht",
      to: "/",
    },
    {
      label: "Profiil",
      to: "/profiles",
    },
<<<<<<< HEAD
    {
      label: "Loo Arve",
      to: "/create-invoice",
    },
    {
      label: "Avaleht",
      to: "/",
    },
=======
>>>>>>> 845bdeefa9a2d4b4454a623393fc596f6a9b5796
    
  ];
  </script>