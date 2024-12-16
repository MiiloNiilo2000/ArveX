<template>
  <div class="flex justify-center items-center pt-9">
    <div class="w-96 p-6 flex flex-col h-99">
      <h2 class="text-2xl font-bold mb-4 text-center">Muuda ettevõtet</h2>
      <UForm
        :validate="validate"
        :state="state"
        class="space-y-4"
        @submit="onSubmit"
        @error="onError"
      >
        <UFormGroup label="Firma nimi" name="name">
          <UInput
            v-model="state.name"
            color="emerald"
            class="bg-gray-900 rounded-md"
          />
        </UFormGroup>
        <UFormGroup label="Registrikood" name="registerCode">
          <UInput
            v-model="state.registerCode"
            color="emerald"
            class="bg-gray-900 rounded-md"
          />
        </UFormGroup>
        <UFormGroup label="KMKN" name="vatNumber">
          <UInput
            v-model="state.vatNumber"
            color="emerald"
            class="bg-gray-900 rounded-md"
          />
        </UFormGroup>
        <UFormGroup label="Aadress" name="address">
          <UInput
            v-model="state.address"
            color="emerald"
            class="bg-gray-900 rounded-md"
          />
        </UFormGroup>
        <UFormGroup label="Postiindeks" name="postalCode">
          <UInput
            v-model="state.postalCode"
            color="emerald"
            class="bg-gray-900 rounded-md"
          />
        </UFormGroup>
        <UFormGroup label="Riik" name="country">
          <UInput
            v-model="state.country"
            color="emerald"
            class="bg-gray-900 rounded-md"
          />
        </UFormGroup>
        <UFormGroup label="Email" name="email">
          <UInput
            v-model="state.email"
            color="emerald"
            class="bg-gray-900 rounded-md"
          />
        </UFormGroup>
        <UFormGroup label="Pildi link" name="image">
          <UInput
            v-model="state.image"
            color="emerald"
            class="bg-gray-900 rounded-md"
          />
        </UFormGroup>
        <div class="col-span-2 flex justify-center">
          <UButton type="submit"> Salvesta </UButton>
        </div>
      </UForm>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
import type { Company } from "../types/company";
import { reactive, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import { useApi } from "../composables/useApi";

const { customFetch } = useApi();
const route = useRoute();
const router = useRouter();
const companyId = Number(route.params.id);

const state = reactive<Company>({
  companyId: 0,
  name: "",
  registerCode: "",
  vatNumber: "",
  address: "",
  postalCode: "",
  country: "",
  email: "",
  image: "",
});

const editCompany = async (company: Company) => {
  try {
    await customFetch(`Companies/${company.companyId}`, {
      method: "PUT",
      body: company,
    });
    await router.push("/profiles");
  } catch (error) {
    console.error("Error updating company:", error);
  }
};

const fetchCompany = async (id: number) => {
  try {
    const response = await customFetch<Company[]>(
      `Companies/${id}`,
      { method: "GET" }
    );
    const company = response;
    if (company) {
      Object.assign(state, company);
    }
  } catch (error) {
    console.error("Error fetching company:", error);
  }
};

const validate = (state: any): FormError[] => {
  const errors = [];
  if (!state.name) errors.push({ path: "name", message: "Required" });
  if (!state.email) errors.push({ path: "email", message: "Required" });
  if (!state.registerCode)
    errors.push({ path: "registerCode", message: "Required" });
  if (!state.vatNumber) errors.push({ path: "vatNumber", message: "Required" });
  if (!state.address) errors.push({ path: "address", message: "Required" });
  if (!state.postalCode)
    errors.push({ path: "postalCode", message: "Required" });
  if (!state.country) errors.push({ path: "country", message: "Required" });
  if (
    !/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(state.email)
  ) {
    errors.push({ path: "email", message: "Invalid email format" });
  }
  return errors;
};

async function onSubmit(event: FormSubmitEvent<any>) {
  console.log("Submitting company:", { ...state });
  await editCompany({ ...state });
}

async function onError(event: FormErrorEvent) {
  const element = document.getElementById(event.errors[0].id);
  element?.focus();
  element?.scrollIntoView({ behavior: "smooth", block: "center" });
}

onMounted(async () => {
  await fetchCompany(companyId);
});
</script>

<style scoped>
select {
  border: 1.5px solid #38a169;
  background-color: #111827;
  color: white;
  border-radius: 0.375rem;
  padding: 0.2rem 0.75rem;
  font-size: 1rem;
  transition: border-color 0.2s ease-in-out;
}

select:focus {
  border-color: #42ac4e;
  outline: none;
  box-shadow: 0 0 0 0.1rem rgba(66, 248, 56, 0.413);
}

.form-field {
  border: 1px solid #38a169;
  background-color: #121212;
  color: black;
  border-radius: 0.375rem;
  padding: 0.375rem 0.75rem;
  font-size: 1rem;
  width: 100%;
  height: 3rem;
  transition: border-color 0.2s ease-in-out;
}

.form-field:focus {
  border-color: #2f855a;
  outline: none;
  box-shadow: 0 0 0 0.2rem rgba(56, 189, 248, 0.25);
}
</style>
