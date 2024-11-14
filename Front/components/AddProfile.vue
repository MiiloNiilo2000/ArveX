<template>
  <div class="flex justify-center items-center pt-9">
    <div class="w-96 p-6 flex flex-col h-auto">
      <h2 class="text-2xl font-bold mb-4 text-center">Lisa kasutaja</h2>
      <UForm
        :validate="validate"
        :state="state"
        class="space-y-4"
        @submit="onSubmit"
        @error="onError"
      >
        <UFormGroup label="Kasutajanimi" name="username">
          <UInput v-model="state.username" />
        </UFormGroup>
        
        <UFormGroup label="E-post" name="email">
          <UInput v-model="state.email" type="email" />
        </UFormGroup>

        <UFormGroup label="Pildi link">
          <UInput v-model="state.image" />
        </UFormGroup>

        <UButton type="submit"> Lisa </UButton>
      </UForm>
    </div>
  </div>
</template>

  <script setup lang="ts">
  import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
  import type { Profile } from "../types/profile";
  import { useProfileStore } from '../stores/profileStores';
  import { reactive } from "vue";

  const profileStore = useProfileStore();
  
  const state = reactive<Profile>({
    id: 0,
    username: '',
    email: '',
    image: '',
  });
  
  const validate = (state: any): FormError[] => {
    const errors = [];
    if (!state.username) errors.push({ path: "username", message: "Required" });
    if (!state.email) errors.push({ path: "email", message: "Required"   
   });
    if (!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(state.email)) {
      errors.push({ path: "email", message: "Invalid email format" });
    }
    return errors;
  };
  
  async function onSubmit(event: FormSubmitEvent<any>) {
    profileStore.addProfile({ ...state });
    await navigateTo("/profiles");
  }
  
  async function onError(event: FormErrorEvent) {
    const element = document.getElementById(event.errors[0].id);
    element?.focus();
    element?.scrollIntoView({ behavior: "smooth", block: "center" });
  }
  </script>