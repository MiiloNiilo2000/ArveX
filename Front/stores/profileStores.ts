import type { Profile } from "../types/profile";
import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useProfileStore = defineStore('profile', () => {
  let i: number = 1;
  
  const profiles = ref<Profile[]>([
    { username: 'Sviit Hõum', 
        bio: 'Test, test', 
        email: 'sviithoum@email.com', 
        id: i++,
        image: 'https://i.pinimg.com/736x/c0/74/9b/c0749b7cc401421662ae901ec8f9f660.jpg'
     },
     { username: 'Urmas', 
      bio: 'Tere, tere', 
      email: 'urmas@urmas.com', 
      id: i++,
      image: 'https://i.pinimg.com/736x/c0/74/9b/c0749b7cc401421662ae901ec8f9f660.jpg',
   },
  ]);

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