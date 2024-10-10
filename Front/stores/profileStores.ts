import type { Profile } from "~/types/profile";
import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useProfileStore = defineStore('profile', () => {
  let i: number = 1;
  
  const profiles = ref<Profile[]>([
    { username: 'Sviit Hõum', 
        bio: 'Test, test', 
        email: 'sviithoum@email.com', 
        id: i++,
        image: 'https://wallpapers.com/images/hd/funny-profile-picture-1l2l3tmmbobjqd53.jpg'
     },
     { username: 'Urmas', 
      bio: 'Tere, tere', 
      email: 'urmas@urmas.com', 
      id: i++,
      image: 'https://i.pinimg.com/280x280_RS/e1/6e/03/e16e034cbae22142e26679c6ef5b616e.jpg',
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