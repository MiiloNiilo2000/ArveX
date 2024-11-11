import type { Profile } from "../types/profile";
import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useProfileStore = defineStore('profile', () => {
  let i: number = 1;
  
  const profiles = ref<Profile[]>([]);

  const addProfile = (profile: Profile) => {
    profile.id = i++;
    profiles.value.push(profile);
  };

  const deleteProfile = (id: number) => {
    const index = profiles.value.findIndex((profile) => profile.id === id);
    if (index !== -1) {
      profiles.value.splice(index, 1);
    }
  };

  return { profiles, addProfile, deleteProfile };
});