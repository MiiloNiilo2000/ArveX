<template>
  <div>

    <input 
      type="text"
      v-model="searchTerm"
      placeholder="Otsi..."
      class="form-search mt-4"
      />

    <UTable :columns="columns" :rows="filteredInvoices" class="mt-4">

      <template #header="{ column }">
        <span @click="toggleSort(column)" :class="{ 'cursor-pointer': column.sortable }">
          {{ column.label }}
          <span v-if="sortState.key === column.key">
            {{ sortState.order === 'asc' ? '▲' : '▼' }}
          </span>
        </span>
      </template>

      <template #title-data="{ row }">
        <span class="text-emerald-500 font-bold">
          {{ row.title }}
        </span>
      </template>

      <template #invoiceNumber-data="{ row }">
        <span class="text-indigo-400">
          {{ row.invoiceNumber }}
        </span>
      </template>

      <template #dateCreated-data="{ row }">
        <span class="text-gray-400">
          {{ new Date(row.dateCreated).toLocaleDateString() }}
        </span>
      </template>

      <template #dateDue-data="{ row }">
        <span :class="{'text-red-500 font-bold': isPastDue(row.dateDue), 'text-green-500': !isPastDue(row.dateDue)}">
          {{ new Date(row.dateDue).toLocaleDateString() }}
        </span>
      </template>

      <template #view-data="{ row }">
        <UButton 
        @click="viewInvoice(row)" 
        color="gray" 
        variant="ghost" 
        icon="i-heroicons-arrow-down-tray" 
        />
      </template>

      <template #delete-data="{ row }">
        <UButton 
          @click="deleteInvoice(row.id)" 
          color="gray" 
          variant="ghost" 
          icon="i-heroicons-trash" 
        />
      </template>

      
    </UTable>
  </div>
</template>
  
<script setup lang="ts">
  import { ref, onMounted, computed } from 'vue';
  import { generateInvoicePDF } from '../stores/invoiceStores'; 
  import type { CompanyInvoice } from "../types/companyInvoice";
  import type { PrivatePersonInvoice } from "../types/privatePersonInvoice";
  import { useApi } from '../composables/useApi';
  import { RefSymbol } from '@vue/reactivity';

  const companyInvoices = ref<CompanyInvoice[]>([]);
  const privatePersonInvoices = ref<PrivatePersonInvoice[]>([]);
  const { customFetch } = useApi();

  const columns = ref([
    { key: 'title', label: 'Nimi', sortable: true },
    { key: 'invoiceNumber', label: 'Arve Number', sortable: true },
    { key: 'dateCreated', label: 'Loomiskuupäev', sortable: true },
    { key: 'dateDue', label: 'Tähtaeg', sortable: true },
    { key: 'view', label: 'Laadi Alla' },
    { key: 'delete', label: 'Kustuta' },
  ]);

  const searchTerm = ref('');
  const invoiceType = ref<'company' | 'privateperson' | 'all'>('all');

  const filteredInvoices = computed(() => {
    let allInvoices = [...companyInvoices.value, ...privatePersonInvoices.value];

    if (!searchTerm.value) return allInvoices;

    const query = searchTerm.value.toLowerCase();
    return allInvoices.filter(invoice => {
      return (
        invoice.title.toLowerCase().includes(query) ||
        invoice.invoiceNumber.toString().includes(query) ||
        (invoice as any).clientKMKR?.toLowerCase().includes(query) ||
        (invoice as any).clientRegNr?.toLowerCase().includes(query) ||
        (invoice as any).country?.toLowerCase().includes(query) ||
        new Date(invoice.dateCreated).toLocaleDateString().includes(query) ||
        new Date(invoice.dateDue).toLocaleDateString().includes(query)
      );
    });
  });


  const sortState = ref<{ key: keyof CompanyInvoice | null; order: 'asc' | 'desc' }>({
    key: null,
    order: 'asc',
  });

  type Column = {
    key: keyof CompanyInvoice | keyof PrivatePersonInvoice | string;
    label: string;
    sortable?: boolean;
  }

  const sortedInvoices = computed(() => {
    if (!sortState.value.key) return invoiceType.value === 'company' ? companyInvoices.value : privatePersonInvoices.value;

    const key = sortState.value.key as keyof (CompanyInvoice | PrivatePersonInvoice);
    const order = sortState.value.order === 'asc' ? 1 : -1;

    const invoices = invoiceType.value === 'company' ? companyInvoices.value : privatePersonInvoices.value;

    return [...invoices].sort((a, b) => {
      const aValue = a[key];
      const bValue = b[key];

      if (aValue < bValue) return -1 * order;
      if (aValue > bValue) return 1 * order;
      return 0;
    });
  });


  const toggleSort = (column: Column) => {
    if (!column.sortable) return;

    if (sortState.value.key === column.key) {
      sortState.value.order = sortState.value.order === 'asc' ? 'desc' : 'asc';
    } else {
      sortState.value.key = column.key as keyof CompanyInvoice;
      sortState.value.order = 'asc';
    }
  };

  const isPastDue = (dueDate: string): boolean => {
    return new Date(dueDate) < new Date();
  };

  const fetchInvoices = async () => {
    try {
      if (invoiceType.value === 'all') {
        const allInvoices = await customFetch<(CompanyInvoice | PrivatePersonInvoice)[]>(`InvoiceHistory/all`);
        companyInvoices.value = allInvoices
                                      .filter(inv => inv.invoiceType === 'company') as CompanyInvoice[];
        privatePersonInvoices.value = allInvoices
                                      .filter(inv => inv.invoiceType === 'privatePerson') as PrivatePersonInvoice[];
      } else {
        const response = await customFetch<CompanyInvoice[] | PrivatePersonInvoice[]>(`InvoiceHistory/all?invoiceType=${invoiceType.value}`);

        if (invoiceType.value === 'company') {
          companyInvoices.value = response as CompanyInvoice[];
        } else {
          privatePersonInvoices.value = response as PrivatePersonInvoice[];
        }
      }
    } catch (error) {
      console.error("Error fetching invoices:", error);
    }
  };

  const deleteInvoice = async (id: number) => {
  try {
    const invoiceToDeleteFromCompany = companyInvoices.value.find(invoice => invoice.id === id);  
    const invoiceToDeleteFromPrivatePerson = privatePersonInvoices.value.find(invoice => invoice.id === id); 

    if (invoiceToDeleteFromCompany || invoiceToDeleteFromPrivatePerson) {

      const url = `InvoiceHistory/${id}`;

      await customFetch(url, { method: 'DELETE' });

      if (invoiceToDeleteFromCompany) {
        companyInvoices.value = companyInvoices.value.filter(invoice => invoice.id !== id); 
      }
      if (invoiceToDeleteFromPrivatePerson) {
        privatePersonInvoices.value = privatePersonInvoices.value.filter(invoice => invoice.id !== id); 
      }
    } else {
      console.error(`Invoice with ID ${id} not found for deletion.`);
    }
  } catch (error) {
    console.error("Error deleting invoice:", error);
  }
};

  const viewInvoice = async (row: any) => {
    let productsAndQuantities = {};
    try {
      productsAndQuantities = row.productsAndQuantitiesJson ? JSON.parse(row.productsAndQuantitiesJson) : {};
    } catch (error) {
      console.error("Error parsing productsAndQuantitiesJson:", error);
    }

    const state = {
      title: row.title,
      clientRegNr: row.clientRegNr,
      clientKMKR: row.clientKMKR,
      address: row.address,
      zipCode: row.zipCode,
      country: row.country,
      invoiceNumber: row.invoiceNumber,
      dateCreated: row.dateCreated,
      dateDue: row.dateDue,
      condition: row.condition,
      delayFine: row.delayFine,
      selectedFont: row.font,
      invoiceType: row.invoiceType,
      productsAndQuantities: productsAndQuantities,
    };

    const route = state.invoiceType === "privatePerson" ? "privatePerson" : "company";
    const routeToSend = route + "WithoutSaving"
    generateInvoicePDF(state, routeToSend);
  };

  onMounted(() => {
    fetchInvoices();
  });
  </script>
  
<style>
  @import '../assetsFront/styles/main.css';
</style>