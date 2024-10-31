import type { Company } from "../types/company";
import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useCompanyStore = defineStore('company', () => {
  let i: number = 1;
  
  const companies = ref<Company[]>([
    {
        id: i++,
        name: 'Ettevõte 1',
        registerCode: 12345678,
        VATnumber: 'EE123456789',
        address: 'Tänav 1, Tallinn',
        postalCode: 10123,
        country: 'Eesti',
        email: 'ettevote1@mail.ee',
        image: 'https://img.freepik.com/premium-vector/minimalist-logo-design-any-corporate-brand-business-company_1253202-77511.jpg'
      },
      {
        id: i++,
        name: 'Ettevõte 2',
        registerCode: 87654321,
        VATnumber: 'EE987654321',
        address: 'Tänav 2, Tartu',
        postalCode: 50403,
        country: 'Eesti',
        email: 'ettevote2@mail.ee',
        image: 'https://marketplace.canva.com/EAE0rNNM2Fg/1/0/1600w/canva-letter-c-trade-marketing-logo-design-template-r9VFYrbB35Y.jpg'
      },
  ]);

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