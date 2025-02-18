<script setup>
import MovieTable from './Components/MovieTable.vue';
</script>

<template>
  <MovieTable />
</template>

<style>
:root {
  --primary: #3B8686;
  --secondary: #79BD9A;
  --success: #28a745;
  --info: #79BD9A;
  --warning: #ffc107;
  --danger: #dc3545;
  --light: #f8f9fa;
  --dark: #343a40;
}

body {
  background-color: var(--light) !important;
}

.btn-primary {
  background-color: var(--primary) !important;
  border: 2px solid var(--primary) !important;
  border-radius: 30px !important;

}

.btn-primary:hover {
  background-color: var(--light) !important;
  color: var(--primary) !important;
}

.btn-secondary {
  background-color: var(--light) !important;
  border: 2px solid var(--primary) !important;
  color: var(--primary) !important;
  border-radius: 30px !important;
}

.btn-secondary:hover {
  background-color: var(--primary) !important;
  color: var(--light) !important;
}

.btn-danger {
  background-color: var(--danger) !important;
  border: 2px solid var(--danger) !important;
  color: var(--light) !important;
  border-radius: 30px !important;
}

.btn-danger:hover {
  background-color: var(--light) !important;
  color: var(--danger) !important;

}

h1 {
  font-size: 3rem !important;
  text-align: center;
  font-weight: bold !important;
}

h2 {
  color: var(--primary);
  text-align: center;
}

.error-message {
  color: var(--danger);
}

</style>
