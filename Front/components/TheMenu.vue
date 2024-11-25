<template>
  <div class="flex justify-between items-center border-b border-gray-200 dark:border-gray-800">
    <UHorizontalNavigation
      :links="filteredLinks"
      class="flex"
    />
    
    <NuxtLink
      v-if="!isLoggedIn"
      to="/login"
      class="bg-blue-500 text-white px-8 py-2 rounded-lg shadow-md font-semibold hover:bg-blue-600"
    >
      Sisselogimine
    </NuxtLink>

    <button
      v-if="isLoggedIn"
      @click="handleLogout"
      class="bg-red-500 text-white px-8 py-2 rounded-lg shadow-md font-semibold hover:bg-red-600 ml-4"
    >
      Väljalogimine
    </button>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue';
import { useRouter } from 'vue-router';
import axios from 'axios';

const isLoggedIn = ref(false);
const router = useRouter();

const links = [
  {
    label: "Avaleht",
    to: "/",
  },
  {
    label: "Ettevõtted",
    to: "/profiles",
    requiresAuth: true,
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
    label: "Profiil",
    to: "/user-profile"
  }
];

const filteredLinks = computed(() =>
  links.filter((link) => !link.requiresAuth || isLoggedIn.value)
);

const checkLoginStatus = () => {
  if (localStorage.getItem('token')) {
    isLoggedIn.value = true;
    axios.defaults.headers['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
  } else {
    isLoggedIn.value = false;
  }
};

onMounted(() => {
  checkLoginStatus();
});

watch(() => localStorage.getItem('token'), (newToken) => {
  if (newToken) {
    isLoggedIn.value = true;
    axios.defaults.headers['Authorization'] = `Bearer ${newToken}`;
  } else {
    isLoggedIn.value = false;
  }
});

const handleLogout = () => {
  localStorage.removeItem('token');
  delete axios.defaults.headers['Authorization'];
  isLoggedIn.value = false;
  router.push('/login');
};

const handleLogin = async (token: string) => {
  localStorage.setItem('token', token);
  axios.defaults.headers['Authorization'] = `Bearer ${token}`;
  isLoggedIn.value = true;
};
</script>

<style scoped>
</style>
