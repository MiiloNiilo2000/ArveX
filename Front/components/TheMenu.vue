<template>
  <div class="flex justify-start items-center border-b border-gray-200 dark:border-gray-800">
    <UHorizontalNavigation
      :links="links"
      class="flex"
    />
    <NuxtLink
      v-if="!isLoggedIn"
      to="/login"
      class="bg-blue-500 text-white px-8 py-2 rounded-lg shadow-md font-semibold hover:bg-blue-600"
    >
      Sisselogimine
    </NuxtLink>
    <NuxtLink
      v-else
      to="/profiles"
      class="bg-blue-500 text-white px-8 py-2 rounded-lg shadow-md font-semibold hover:bg-blue-600"
    >
      Profiil
    </NuxtLink>
  </div>
</template>

<script setup lang="ts">
import { useRoute } from 'vue-router';
import { ref, computed } from 'vue';

const isLoggedIn = computed(() => {
  return localStorage.getItem('token') !== null;
});

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
  {
    label: "Ajalugu",
    to: "/view-history",
  },
  {
    label: "Test",
    to: "/test-home",
  }
];
</script>
