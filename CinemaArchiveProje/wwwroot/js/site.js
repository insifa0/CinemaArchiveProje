<script>
    function scrollGenreRow(button, direction) {
        const container = button.closest('.genre-section').querySelector('.scrollable-row');
    const card = container.querySelector('.film-card');
    if (!card) return;

    const scrollAmount = card.offsetWidth + 20; // 20: CSS gap değeri
    if (direction === 'left') {
        container.scrollBy({ left: -scrollAmount, behavior: 'smooth' });
        } else {
        container.scrollBy({ left: scrollAmount, behavior: 'smooth' });
        }
    }
</script>