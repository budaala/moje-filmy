<script setup>
import { onMounted, onUnmounted, ref } from 'vue';
import { eventBus } from "../eventBus";
import DeleteMovieModal from './DeleteMovieModal.vue';
import MovieModal from './MovieModal.vue';
import MovieService from '@/services/MovieService';

const movies = ref(null);
const successMessage = ref(null);
const errorMessage = ref(null);

// gettin movies
const getMovies = async () => {
    const response = await MovieService.getMovies();
    movies.value = await response.data;
};

const getNewMovies = async () => {
    const response = await MovieService.getNewMovies();
    if (response.success === true) {
        successMessage.value = "Pobrano nowe filmy.";
        getMovies();
    }
    else {
        errorMessage.value = response.errorMessage;
    }
};

// deleting movies
const openDeleteMovieModal = (movieIdToDelete, movieTitleToDelete) => {
    eventBus.emit("openDeleteMovieModal", { movieId: movieIdToDelete, movieTitle: movieTitleToDelete });
};

const movieDeleted = (movieId) => {
    successMessage.value = "Film o id " + movieId + " został usunięty.";
    console.log("getingMovies");
    getMovies();
};

// adding and editing movies
const openMovieModal = (mode, movie = null) => {
    eventBus.emit("openMovieModal", { mode: mode, movie: movie });
};

onMounted(() => {
    getMovies();
    eventBus.on("movieDeleted", movieDeleted);
    eventBus.on("movieAdded", () => {
        successMessage.value = "Film został dodany.";
        getMovies();
    });
    eventBus.on("movieEdited", () => {
        successMessage.value = "Film został zaktualizowany.";
        getMovies();
    });
});

onUnmounted(() => {
    eventBus.off("movieDeleted");
    eventBus.off("movieAdded");
    eventBus.off("movieEdited");
});

</script>

<template>
    <div class="container-md">
        <h1>Biblioteka filmów</h1>
        <div class="action-buttons">
            <button class="btn btn-primary" id="download-button" @click="getNewMovies">Pobierz filmy</button>
            <button class="btn btn-secondary" id="add-button" @click="openMovieModal('add')">Dodaj</button>
        </div>
        <div v-if="successMessage" class="alert alert-success alert-dismissible fade show" role="alert">
            {{ successMessage }}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
        <div v-if="errorMessage" class="alert alert-danger alert-dismissible fade show" role="alert">
            {{ errorMessage }}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
        <table class="table">
            <thead>
                <tr>
                    <th scope="col">Id</th>
                    <th scope="col">Tytuł</th>
                    <th scope="col">Reżyser</th>
                    <th scope="col">Rok</th>
                    <th scope="col">Ocena</th>
                    <th scope="col">Akcje</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(movie) in movies" :key="movie.id">
                    <th scope="row">{{ movie.id }}</th>
                    <td>{{ movie.title }}</td>
                    <td>{{ movie.director }}</td>
                    <td>{{ movie.year }}</td>
                    <td>{{ movie.rate }}</td>
                    <td class="row-buttons">
                        <button class="btn btn-secondary" @click="openMovieModal('edit', movie)">Edytuj</button>
                        <button class="btn btn-danger"
                            @click="openDeleteMovieModal(movie.id, movie.title)">Usuń</button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <DeleteMovieModal />

    <MovieModal />

</template>

<style scoped>
.container-md {
    margin: 5rem auto;
    padding: 2rem;
}

.action-buttons {
    display: flex;
    justify-content: center;
    gap: 1rem;
    margin: 2rem 0;
}

#download-button,
#add-button {
    padding: 8px 50px !important;
}

table {
    margin: 1rem auto;
}

.row-buttons {
    display: flex;
    gap: 0.5rem;
}
</style>