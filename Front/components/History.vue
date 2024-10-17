<template>
    <UTable :columns="columns" :rows="invoices" />
  </template>

<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';

const invoices = ref([]);

const columns = ref([
    { key: 'title', label: 'Firma Nimi' },
    { key: 'invoiceNumber', label: 'Invoice Number' },
    { key: 'dateCreated', label: 'Date Created' },
    { key: 'dateDue', label: 'Due Date' },
]);

const fetchInvoices = async () => {
    try {
        const response = await axios.get('http://localhost:5176/InvoiceHistory/all');
        invoices.value = response.data;
    }
    catch (error){
        console.error("Error fetching invoices:", error);
    }
};

onMounted(() => {
    fetchInvoices();
});

</script>