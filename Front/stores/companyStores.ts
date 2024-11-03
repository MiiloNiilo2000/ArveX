import type { Company } from "../types/company";
import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useCompanyStore = defineStore('company', () => {
  let i: number = 1;
  
  const companies = ref<Company[]>([]);

  const addCompany = (company: Company) => {
    company.id = i++;
    companies.value.push(company);
  };

  const deleteCompany= (id: number) => {
    const index = companies.value.findIndex((company) => company.id === id);
    if (index !== -1) {
      companies.value.splice(index, 1);
    }
  };

  return { companies, addCompany, deleteCompany };
});