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
      label: "Home",
      to: "/",
    },
    {
      label: "Invoice",
      to: "/add-invoice",
    },
  ];
  </script>