<template>
  <div class="flex justify-between items-center border-b border-gray-200 dark:border-gray-800 px-4 py-2">
    <UHorizontalNavigation
      :links="filteredLinks"
      class="flex"
    />
    
    <div v-if="isLoggedIn" class="relative">
      <button @click="toggleDropdown" class="focus:outline-none">
        <img
          :src="profile?.image || defaultProfileImage"
          alt="User profile"
          class="w-10 h-10 rounded-full border-2 border-gray-300"
        />
      </button>

      <div
        v-if="showDropdown"
        class="absolute right-0 mt-2 w-48 bg-white border border-gray-200 rounded-lg shadow-lg z-10"
      >
        <div class="px-4 py-3 border-b border-gray-200">
          <p class="text-sm font-semibold text-gray-800">
            {{ profile?.username }}
          </p>
          <p class="text-xs text-gray-500">{{ profile?.email }}</p>
        </div>
        <ul>
          <li>
            <button
              @click="handleLogout"
              class="w-full text-left px-4 py-2 text-sm text-red-500 hover:bg-red-50"
            >
              Väljalogimine
            </button>
          </li>
        </ul>
      </div>
    </div>

    <NuxtLink
      v-else
      to="/login"
      class="bg-green-500 text-white px-8 py-1.5 rounded-lg shadow-md font-semibold mt-1 hover:bg-green-600"
    >
      Sisselogimine
    </NuxtLink>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue';
import { useRouter } from 'vue-router';
import axios from 'axios';

const isLoggedIn = ref(false);
const showDropdown = ref(false);

const defaultProfileImage =
  "https://media.istockphoto.com/id/1300845620/vector/user-icon-flat-isolated-on-white-background-user-symbol-vector-illustration.jpg?s=612x612&w=0&k=20&c=yBeyba0hUkh14_jgv1OKqIH0CCSWU_4ckRkAoy2p73o=";

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
];

const profile = ref<{
  username?: string;
  email?: string;
  image?: string;
} | null>(null);

const filteredLinks = computed(() =>
  links.filter((link) => !link.requiresAuth || isLoggedIn.value)
);
const { customFetch } = useApi();

const checkLoginStatus = async () => {
  if (localStorage.getItem("token")) {
    isLoggedIn.value = true;
    axios.defaults.headers["Authorization"] = `Bearer ${localStorage.getItem("token")}`;
    await fetchUserProfile();
  } else {
    isLoggedIn.value = false;
  }
};

const fetchUserProfile = async () => {
  try {
    const response = await customFetch<{
      token: string,
      id: string;
      username?: string;
      email?: string;
    }>('Profile/Me', {
      method: 'GET',
    });
    profile.value = response;
  } catch (error) {
    console.error('Error fetching profile:', error);
    profile.value = null;
  }
};

onMounted(() => {
  checkLoginStatus();
});

watch(() => localStorage.getItem("token"), async (newToken) => {
  if (newToken) {
    isLoggedIn.value = true;
    axios.defaults.headers["Authorization"] = `Bearer ${newToken}`;
    await fetchUserProfile();
  } else {
    isLoggedIn.value = false;
  }
});

const handleLogout = () => {
  localStorage.removeItem("token");
  delete axios.defaults.headers["Authorization"];
  isLoggedIn.value = false;
  showDropdown.value = false;
  router.push("/login");
};

const toggleDropdown = () => {
  showDropdown.value = !showDropdown.value;
};
</script>

<style scoped>
</style>
