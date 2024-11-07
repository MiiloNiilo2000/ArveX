<template>
    <div class="flex items-center justify-center min-h-screen">
      <div class="hexagon-container">

        <div class="hexagon-line top"></div>

        <div class="hexagon-line top-right"></div>

        <div class="hexagon-line bottom-right"></div>

        <div class="hexagon-line bottom"></div>

        <div class="hexagon-line bottom-left"></div>

        <div class="hexagon-line top-left"></div>

        <NuxtLink
            to="/create-invoice"
            class="centered-top-button text-white 
            bg-gradient-to-r from-green-400 via-green-500 to-green-600 
            hover:bg-gradient-to-br 
            focus:ring-4 
            focus:outline-none 
            focus:ring-green-300 
            dark:focus:ring-green-800 shadow-lg shadow-green-500/50 
            dark:shadow-lg 
            dark:shadow-green-800/80 
            font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2">
            Alusta uue arve loomist
        </NuxtLink>   

        <div class="login-text">
            Kõikide funktsioonide kasutamiseks peate olema sisse logitud
        </div>

        <NuxtLink
            to="/signup"
            class="centered-button text-white 
            bg-gradient-to-r from-green-400 via-green-500 to-green-600 
            hover:bg-gradient-to-br 
            focus:ring-4 
            focus:outline-none 
            focus:ring-green-300 
            dark:focus:ring-green-800 shadow-lg shadow-green-500/50 
            dark:shadow-lg 
            dark:shadow-green-800/80 
            font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2">
            Logi sisse / Registreeri
        </NuxtLink>   
        
        <NuxtLink to="/products">
            <UButton class="icon-button icon-button-1" icon="mdi-box-variant" />
        </NuxtLink>

        <NuxtLink to="/view-history">
            <UButton class="icon-button icon-button-2" icon="mdi-eye" />
        </NuxtLink>
        
        <div class="top-text">
            Arve<span class="last-letter">X</span>
        </div>

        <div class="product-text">
            Halda tooteid
        </div>

        <div class="history-text">
            Vaata ajalugu
        </div>
    
    </div>

    </div>
  </template>
  
  <script>
    import { NuxtLink } from "#components";
  export default {
    name: "Hexagon",
  };

  </script>
  
<style scoped>

.hexagon-container {
    position: relative;
    width: 50px;
    height: 1000px;
}

.hexagon-line {
    position: absolute;
    width: 288px; 
    height: 2px;
    background-color: #38a169;
    box-shadow: 0 0 8px #38a169, 0 0 12px #00f, 0 0 16px #38a169;
}

.top {
    top: 0;
    left: 50%;
    transform: translateX(-50%);
}

.top-right {
    top: 25%;
    right: 0;
    transform: rotate(60deg);
    transform-origin: left center;
}

.bottom-right {
    bottom: 25%;
    right: 0;
    transform: rotate(-60deg);
    transform-origin: left center;
}

.bottom {
    bottom: 0;
    left: 50%;
    transform: translateX(-50%);
}

.bottom-left {
    bottom: 25%;
    left: 0;
    transform: rotate(60deg);
    transform-origin: right center;
}

.top-left {
    top: 25%;
    left: 0;
    transform: rotate(-60deg);
    transform-origin: right center;
}

.top-text {
    font-size: 100px;
    position: absolute;
    top: -115px;
    transform: translateX(-50%);
    left: 50%;
    font-family: 'LowBallItalicBold';
    color: #33cd35;
    user-select: none;
}

.last-letter {
    color: #9d00ff; 
    text-shadow: 0 0 8px #9d00ff;
}

.centered-top-button {
    position: absolute;
    width: 200px;
    top: 20px;
    left: 50%;
    transform: translateX(-50%);
    font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.centered-button {
    position: absolute;
    width: 200px;
    top: 270px;
    left: 50%;
    transform: translateX(-50%);
    font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.icon-button-base {
    color: none;
    background: #38a169;
    border: none;
    cursor: pointer;
    border-radius: 50%; 
    padding: 10px; 
    transition: box-shadow 0.2s, transform 0.2s;
}

.icon-button:hover {
    box-shadow: 0 4px 8px #38a169;
}

.icon-button-1 {
    position: absolute;
    top: 200px;
    left: -254px;
}

.icon-button-2 {
    position: absolute;
    top: 200px;
    left: 268px;
}

.product-text{
    position: absolute;
    top: 170px; 
    left: -284px;
    font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    user-select: none;
}

.history-text{
    position: absolute;
    top: 170px;
    width: 100px; 
    left: 240px;
    font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    user-select: none;
}

.login-text{
    position: absolute;
    text-align: center;
    top: 215px;
    width: 260px; 
    left: -106px;
    font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    user-select: none;
}

@font-face {
font-family: 'LowBallItalicBold';
src: url('/assetsFront/fonts/LowballNeueBoldItalic-lgXJw.ttf') format('truetype');
font-weight: bold;
font-style: normal;
}

</style>
  
  