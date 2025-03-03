<script setup>
import { onMounted, onUnmounted, ref } from 'vue';
import { eventBus } from "../eventBus";
import DeleteMovieModal from './DeleteMovieModal.vue';
import MovieModal from './MovieModal.vue';
import MovieService from '@/services/MovieService';

const movies = ref(null);
const successMessage = ref(null);
const errorMessage = ref(null);
let loading = ref(true);

// gettin movies
const getMovies = async () => {
    const response = await MovieService.getMovies();
    if (response.success === false) {
        errorMessage.value = response.errorMessage;
        loading.value = false;
        return;
    }
    if (response.data.length === 0) {
        errorMessage.value = "Brak filmów w bazie.";
        loading.value = false;
        return;
    }
    movies.value = await response.data;
    loading.value = false;
};

const getNewMovies = async () => {
    const response = await MovieService.getNewMovies();
    if (response.success === true) {
        successMessage.value = response.successMessage;
        closeSuccessAlert();
        getMovies();
    }
    else {
        errorMessage.value = response.errorMessage;
    }
};

// close success alert after 5 seconds
const closeSuccessAlert = () => {
    if (successMessage.value === null) {
        return;
    }
    setTimeout(() => {
        successMessage.value = null;
    }, 5000);
};

// deleting movies
const openDeleteMovieModal = (movieIdToDelete, movieTitleToDelete) => {
    eventBus.emit("openDeleteMovieModal", { movieId: movieIdToDelete, movieTitle: movieTitleToDelete });
};

const movieDeleted = (movieId) => {
    successMessage.value = "Film o id " + movieId + " został usunięty.";
    closeSuccessAlert();
    getMovies();
};

// adding and editing movies
const openMovieModal = (mode, movie = null) => {
    eventBus.emit("openMovieModal", { mode: mode, movie: movie });
};

const closeAlert = () => {
    successMessage.value = null;
};

onMounted(() => {
    getMovies();
    eventBus.on("movieDeleted", movieDeleted);
    eventBus.on("movieAdded", () => {
        successMessage.value = "Film został dodany.";
        closeSuccessAlert();
        getMovies();
    });
    eventBus.on("movieEdited", () => {
        successMessage.value = "Film został zaktualizowany.";
        closeSuccessAlert();
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
            <button class="btn btn-primary" id="add-button" @click="openMovieModal('add')">Dodaj</button>
        </div>
        <div v-if="successMessage" class="alert alert-success alert-dismissible fade show" role="alert">
            {{ successMessage }}
            <button type="button" class="btn-close" data-bs-dismiss="alert" @click="closeAlert" aria-label="Close"></button>
        </div>
        <div v-if="errorMessage" class="alert alert-danger alert-dismissible fade show" role="alert">
            {{ errorMessage }}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
        <div class="table-responsive">
            <table class="table table-light table-striped">
                <thead>
                    <tr>
                        <th scope="col">Tytuł</th>
                        <th scope="col">Reżyser</th>
                        <th scope="col">Rok</th>
                        <th scope="col">Ocena</th>
                        <th scope="col">Akcje</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(movie) in movies" :key="movie.id">
                        <td>{{ movie.title }}</td>
                        <td>{{ movie.director }}</td>
                        <td>{{ movie.year }}</td>
                        <td>{{ movie.rate }}</td>
                        <td class="row-buttons">
                            <button class="btn btn-secondary edit-button" @click="openMovieModal('edit', movie)">Edytuj</button>
                            <button class="btn btn-danger delete-button"
                                @click="openDeleteMovieModal(movie.id, movie.title)">Usuń</button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div v-if="loading === true" class="d-flex justify-content-center">
            <div class="spinner-border" role="status">
                <span class="visually-hidden">Loading...</span>
            </div>
        </div>
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

.edit-button {
    margin-right: 0.5rem;
}

.table-responsive {
    margin: 1rem auto;
    border: 1px solid #ccc;
    border-radius: 10px;
    padding: 1rem;
}

@media screen and (max-width: 670px) {
    .row-buttons {
        display: flex;
        flex-direction: column;
        gap: 0.5rem;
    }
    
}

@media screen and (max-width: 500px) {
    h1 {
        font-size: 2rem !important;
    }

    .action-buttons {
        flex-direction: column;
        gap: 0.5rem;
    }

}
</style>