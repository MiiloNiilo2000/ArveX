<template>
  <div class="min-h-screen">

    <section class="bg-gradient-to-b from-green-600 text-white py-20 ">
      <div class="container mx-auto px-6 text-center">
        <h1 class="text-4xl font-bold mb-4">ArveX</h1>
        <p class="text-lg mb-8">Loo arveid kiiresti ja mugavalt.</p>
        <NuxtLink
          to="/create-invoice"
          class="bg-white text-black px-6 py-3 rounded-lg shadow-md font-semibold hover:bg-gray-100"
        >
          Proovi kohe
        </NuxtLink>
      </div>
    </section>

    <section class="container mx-auto px-6 py-16 text-gray-700 dark:text-white bg-inherit">
      <h2 class="text-3xl font-bold text-center mb-12">Miks valida ArveX?</h2>
      <div class="grid grid-cols-1 md:grid-cols-3 gap-8">

        <NuxtLink to="/create-invoice" class="p-6 rounded-lg shadow-md bg-white dark:bg-gray-700 transition duration-300 ease-in-out transform hover:scale-105">
          <div class="text-green-600 text-4xl mb-4 text-center">📑</div>
          <h3 class="text-xl font-semibold mb-2 text-center">Lihtne luua</h3>
          <p class="text-gray-600 dark:text-gray-300 text-center">Loo ja kohanda arveid vaid mõne klikiga.</p>
        </NuxtLink>

        <NuxtLink to="/view-history" class="p-6 rounded-lg shadow-md bg-white dark:bg-gray-700 transition duration-300 ease-in-out transform hover:scale-105">
          <div class="text-green-600 text-4xl mb-4 text-center">📈</div>
          <h3 class="text-xl font-semibold mb-2 text-center">Jälgi arveid</h3>
          <p class="text-gray-600 dark:text-gray-300 text-center">Loodud arved salvestuvad, et saaksid neid hiljem hallata.</p>
        </NuxtLink>

        <NuxtLink to="/products" class="p-6 rounded-lg shadow-md bg-white dark:bg-gray-700 transition duration-300 ease-in-out transform hover:scale-105">
          <div class="text-green-600 text-4xl mb-4 text-center">💼</div>
          <h3 class="text-xl font-semibold mb-2 text-center">Toote- / teenusebaasi lisamine</h3>
          <p class="text-gray-600 dark:text-gray-300 text-center">Loo oma toodetest ja / või teenustest loend ning lisa neid hõlbsalt arvele.</p>
        </NuxtLink>
      </div>
    </section>

    <div class="w-full h-px bg-gray-300 dark:bg-gray-600 my-8"></div>

    <section class="py-16 bg-inherit">
      <div class="container mx-auto px-6">
        <h2 class="text-3xl font-bold text-center mb-12 text-gray-900 dark:text-white">Kuidas luua?</h2>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
          <div class="p-6 rounded-lg shadow-md text-center bg-white dark:bg-gray-700">
            <div class="text-green-600 text-4xl mb-4">1️⃣</div>
            <h3 class="text-xl font-semibold mb-2">Loo oma ettevõtte profiil</h3>
            <p class="text-gray-600 dark:text-gray-300">Lisa oma ettevõtte nimi ja muud andmed, et neid automaatselt arvele kuvada.</p>
          </div>
          <div class="p-6 rounded-lg shadow-md text-center bg-white dark:bg-gray-700">
            <div class="text-green-600 text-4xl mb-4">2️⃣</div>
            <h3 class="text-xl font-semibold mb-2">Lisa tooted</h3>
            <p class="text-gray-600 dark:text-gray-300">Lisa tootebaasi enda firma pakutavad tooted ja teenused.</p>
          </div>
          <div class="p-6 rounded-lg shadow-md text-center bg-white dark:bg-gray-700">
            <div class="text-green-600 text-4xl mb-4">3️⃣</div>
            <h3 class="text-xl font-semibold mb-2">Loo arve ning lae alla</h3>
            <p class="text-gray-600 dark:text-gray-300">Arvel loomisel saad Eesti ettevõtte puhul kliendi andmed täita automaatselt, lisa vaid tooted ning kujundused ja ongi valmis!</p>
          </div>
        </div>
      </div>
    </section>

    <section class="bg-gradient-to-t from-green-600 text-white py-16">
      <div class="container mx-auto px-6 text-center">
        <h2 class="text-3xl font-bold mb-6">Loo arveid juba täna!</h2>
        <p class="text-lg mb-8">Registreeru ja loo arveid nagu professionaal.</p>
        <NuxtLink
          to="/signup"
          class="bg-white text-black px-6 py-3 rounded-lg shadow-md font-semibold hover:bg-gray-100"
        >
          Registreeri
        </NuxtLink>
      </div>
    </section>
  </div>
</template>

<script setup>
import { NuxtLink } from "#components";
</script>

<style scoped>
.container {
  max-width: 1200px;
}

section {
  position: relative;
}

.absolute {
  position: absolute;
}

</style>
