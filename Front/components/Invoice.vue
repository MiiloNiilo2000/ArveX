<template>
    <UForm
      :validate="validate"
      :state="state"
      class="space-y-4"
      @submit.prevent="submitForm"
      @error="onError"
    >

    <div class="flex max-w-lg gap-20"> 
      <div class="w-3/5"> 

        <UFormGroup label="Firma nimi" name="title">
          <UInput v-model="state.title" class="w-full h-12" color="emerald" placeholder="'ArveX...'" />
        </UFormGroup>

        <UFormGroup label="Aadress" name="address">
          <UInput v-model="state.address" class="w-full h-12" color="emerald" placeholder="'Ehitajate Tee 5'" />
        </UFormGroup>

        <UFormGroup label="Linn" name="city">
          <UInput v-model="state.city" class="w-full h-12" color="emerald" placeholder="'Tallinn'"/>
        </UFormGroup>

        <UFormGroup label="Postiindeks" name="zipCode">
          <UInput v-model="state.zipCode" class="w-full h-12" color="emerald" placeholder="'12345'"/>
        </UFormGroup>

        <UFormGroup label="Riik" name="country">
          <UInput v-model="state.country" class="w-full h-12" color="emerald" placeholder="'Eesti'"/>
        </UFormGroup>

      </div>

      <div class="w-2/5"> 

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
      </div>
    </div>

      <UButton type="submit">Generate PDF</UButton>

    </UForm>
</template>

<script setup lang="ts">
    import { reactive } from 'vue';
    import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
    import { useInvoiceStore } from "~/stores/stores";
    import type { Invoice } from "~/types/invoice";
    import axios from 'axios';

    const state = reactive({
      title: '',
      address: '',
      city: '',
      zipCode: '',
      country: '',
      invoiceNumber: '',
      dateCreated: '',
      dateDue: '',
      condition: '',
      delayFine: '',
    });

    const validate = (state: any): FormError[] => {
      const errors = [];
      const zipString = state.zipCode.toString();
      if (!state.title) errors.push({ path: "title", message: "Required" });
      if (!state.address) errors.push({ path: "address", message: "Required" });
      if (!state.city) errors.push({ path: "city", message: "Required" });
      if (!state.zipCode) errors.push({ path: "zipCode", message: "Required" });
      if (zipString.length < 5 || zipString.length > 5) errors.push({ path: "zipCode", message: "Postiindeks peab olema 5-kohaline number" });
      if (!state.country) errors.push({ path: "country", message: "Required" });
      return errors;
    };
    
    const submitForm = async () => {
      console.log("Form submitted");
      try {
        const response = await axios.post('http://localhost:5176/CreateInvoice', {
          title: state.title,
          address: state.address,
          city: state.city,
          zipCode: parseInt(state.zipCode),
          country: state.country,
          invoiceNumber: parseInt(state.invoiceNumber),
          dateCreated: new Date(state.dateCreated).toISOString(),
          dateDue: new Date(state.dateDue).toISOString(),
          condition: state.condition || "",
          delayFine: state.delayFine || ""
        }, { responseType: 'blob' });

        const url = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', 'invoice.pdf');
        document.body.appendChild(link);
        link.click();
        link.remove();
      } catch (error) {
        console.error("Error generating PDF:", error);

      }
    };

    async function onError(event: FormErrorEvent) {
      const element = document.getElementById(event.errors[0].id);
      element?.focus();
      element?.scrollIntoView({ behavior: "smooth", block: "center" });
    }

  </script>
