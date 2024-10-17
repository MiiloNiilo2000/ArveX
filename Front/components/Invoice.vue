<template>
  <UForm
    :validate="validate"
    :state="state"
    class="space-y-4"
    @submit.prevent="submitForm"
    @error="onError"
  >
    <div class="flex w-full gap-20"> 
      <div class="w-1/2"> 

        <UFormGroup label="Firma nimi" name="title">
          <UInput
          v-model="state.title"
          @input="fetchCompanyNames"
          class="w-full h-12"
          color="emerald"
          placeholder="Sisesta firma nimi"
          list="company-suggestions"
        />
        <datalist id="company-suggestions">
          <option v-for="company in companySuggestions" :key="company.company_id" :value="company.name">
            {{ company.name }}
          </option>
        </datalist>
        </UFormGroup>

        <UFormGroup label="Aadress" name="address">
          <UInput v-model="state.address" class="w-full h-12" color="emerald" placeholder="'Ehitajate Tee 5'" />
        </UFormGroup>

        <UFormGroup label="Postiindeks" name="zipCode">
          <UInput v-model="state.zipCode" class="w-full h-12" color="emerald" placeholder="'12345'"/>
        </UFormGroup>

        <UFormGroup label="Riik" name="country">
          <UInput v-model="state.country" class="w-full h-12" color="emerald" placeholder="'Eesti'"/>
        </UFormGroup>
      </div>

      <div class="w-1/3"> 
        <UFormGroup label="Arve Number" name="invoiceNr">
          <UInput v-model="state.invoiceNumber" class="w-full h-12" color="emerald" placeholder="'54321'"/>
        </UFormGroup>

        <UFormGroup label="Kuupäev" name="dateCreated">
          <UInput 
            v-model="state.dateCreated" 
            type="date" 
            class="w-full h-12"
            color="emerald"
          />
        </UFormGroup>

        <UFormGroup label="Maksetähtaeg" name="dateDue">
          <UInput 
            v-model="state.dateDue" 
            type="date" 
            class="w-full h-12"
            color="emerald"
          />
        </UFormGroup>

        <UFormGroup label="Tingimused" name="condition">
          <UInput v-model="state.condition" class="w-full h-12" color="emerald" placeholder="'12 kuud'"/>
        </UFormGroup>

        <UFormGroup label="Viivis" name="delayFine">
          <UInput v-model="state.delayFine" class="w-full h-12" color="emerald" placeholder="'5% päevas'"/>
        </UFormGroup>

        <UFormGroup label="Font" name="font">
          <select v-model="state.selectedFont">
            <option v-for="font in fonts" :key="font" :value="font">
              {{ font }}
            </option>
          </select>
        </UFormGroup>
      </div>

      <div class="w-full">
        <h1 class="text-2xl font-bold">{{ 'Arve eelvaade' }}</h1>
          <div class="invoice-preview mt-10 p-6 bg-gray-100 shadow-md border rounded-lg">
            <div class="invoice-header text-center mb-6">
              <h1 class="text-2xl font-bold text-black">{{ state.title || 'Tallinn University of Technology' }}</h1>
            </div>

            <div class="flex justify-between">
              <div class="client-details w-6/12">
                <h2 class="text-lg font-semibold text-black">Klient:</h2>
                <h2 class="text-lg font-semibold text-black">{{ state.title || 'Tallinn University of Technology' }}</h2>
                <p class="text-black">
                  {{ state.address || 'Pärnu mnt 62/1, Kesklinna linnaosa' }}<br>
                  {{ state.zipCode || '10135' }}<br>
                  {{ state.country || 'Estonia' }}
                </p>
              </div>

            <div class="invoice-details w-6/12 text-right">
              
              <div class="flex justify-between mb-2">
                <div class="w-1/2 text-left">
                  <h3 class="text-lg font-semibold text-black">Arve number:</h3>
                  <p class="text-black"><span class="label">Kuupäev:</span></p>
                  <p class="text-black"><span class="label">Tingimused:</span></p>
                  <p class="text-black"><span class="label">Maksetähtaeg:</span></p>
                  <p class="text-black"><span class="label">Viivis:</span></p>
                </div>
                <div class="w-1/2 text-left">
                  <p class="text-lg font-semibold text-black"><span>{{ state.invoiceNumber  || '' }}</span></p>
                  <p class="text-black"><span>{{ state.dateCreated || '' }}</span></p>
                  <p class="text-black"><span>{{ state.condition || '' }}</span></p>
                  <p class="text-black"><span>{{ state.dateDue || '' }}</span></p>
                  <p class="text-black"><span>{{ state.delayFine || '' }}</span></p>
                </div>
              </div>
            </div>
            </div>
          </div>
      </div>
    </div>

    <UButton type="submit">Generate PDF</UButton>
  </UForm>
</template>

<script setup lang="ts">

  import { reactive } from 'vue';
  import type { FormError, FormErrorEvent } from "#ui/types";
  import axios from 'axios';

  interface Company {
    company_id: string;
    reg_code: string;
    name: string;
    legal_address: string;
    zip_code: string;    
  }

  const state = reactive({
    title: '',
    address: '',
    zipCode: '',
    country: 'Eesti',
    invoiceNumber: '',
    dateCreated: '',
    dateDue: '',
    condition: '',
    delayFine: '',
    selectedFont: 'Arial',
  });

  const fonts = [
    'Times New Roman',
    'Arial',
    'Courier New',
    'Georgia',
    'Verdana',
  ]

  const validate = (state: any): FormError[] => {
    const errors = [];
    const zipString = state.zipCode.toString();
    if (!state.title) errors.push({ path: "title", message: "Required" });
    if (!state.address) errors.push({ path: "address", message: "Required" });
    if (!state.zipCode) errors.push({ path: "zipCode", message: "Required" });
    if (zipString.length < 5 || zipString.length > 5) errors.push({ path: "zipCode", message: "Postiindeks peab olema 5-kohaline number" });

    return errors;
  };
  
  const companySuggestions = ref<Company[]>([])

    const fetchCompanyNames = async () => {
       if (state.title.length < 3) return; 
      
       try {
         const response = await axios.get(`https://ariregister.rik.ee/est/api/autocomplete?q=${state.title}`);
         companySuggestions.value = response.data.data;
       } catch (error) {
         console.error('Error fetching company names:', error);
       }
     };

     watch(() => state.title, (newTitle) => {
       const selectedCompany = companySuggestions.value.find(company => company.name === newTitle);
       if (selectedCompany) {
         state.address = selectedCompany.legal_address;
         state.zipCode = selectedCompany.zip_code;
       }
     });
    
    const submitForm = async () => {
      console.log("Form submitted");
      try {

        const response = await axios.post('http://localhost:5176/CreateInvoice', {
          title: state.title,
          address: state.address,
          zipCode: state.zipCode.toString(),
          country: state.country,
          invoiceNumber: parseInt(state.invoiceNumber),
          dateCreated: new Date(state.dateCreated).toISOString(),
          dateDue: new Date(state.dateDue).toISOString(),
          condition: state.condition || "",
          delayFine: state.delayFine || "",
          font: state.selectedFont
        }, { responseType: 'blob' });

        const url = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', 'invoice.pdf');
        document.body.appendChild(link);
        link.click();
        link.remove();
      } 
      catch (error) {
        console.error("Error generating PDF:", error);
      }
    };

  async function onError(event: FormErrorEvent) {
    const element = document.getElementById(event.errors[0].id);
    element?.focus();
    element?.scrollIntoView({ behavior: "smooth", block: "center" });
  }
</script>

<style scoped>
  .invoice-preview {
    max-width: 600px;
    background: #f9f9f9;
    padding: 20px;
    border: 1px solid #ccc;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  }
  .invoice-header h1 {
    font-size: 24px;
  }
  .client-details, .invoice-details {
    margin-bottom: 20px;
  }
  .client-details h2, .invoice-details h3 {
    font-size: 18px;
    margin-bottom: 10px;
  }
  p {
    margin: 5px 0;
  }
  .label {
    font-weight: bold;
    margin-right: 10px;
  }
  .flex {
    display: flex;
  }
  .mb-2 {
    margin-bottom: 8px;
  }

  select {
  border: 1.5px solid #38a169; /* Adjust to emerald color */
  background-color: black; /* Background color */
  color: white; /* Text color */
  border-radius: 0.375rem; /* Match rounded corners */
  padding: 0.375rem 0.75rem; /* Adjust padding */
  font-size: 1rem; /* Match font size */
  transition: border-color 0.2s ease-in-out; /* Smooth transition for border color */
  }

  select:focus {
    border-color: #2f855a; /* Darker emerald on focus */
    outline: none; /* Remove default outline */
    box-shadow: 0 0 0 0.2rem rgba(56, 189, 248, 0.25); /* Optional shadow for focus */
  }
  
  .form-field {
  border: 1px solid #38a169; /* Emerald border */
  background-color: white;
  color: black;
  border-radius: 0.375rem;
  padding: 0.375rem 0.75rem;
  font-size: 1rem;
  width: 100%;
  height: 3rem; /* Equivalent to h-12 in Tailwind */
  transition: border-color 0.2s ease-in-out;
  }

  .form-field:focus {
    border-color: #2f855a; /* Darker emerald on focus */
    outline: none;
    box-shadow: 0 0 0 0.2rem rgba(56, 189, 248, 0.25);
  }
</style>
