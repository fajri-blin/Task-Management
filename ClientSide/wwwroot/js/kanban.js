
const cards = document.querySelectorAll('.card');
console.log(cards);

let dragStartTaskId;

function handleDragStart(e) {
    console.log('Drag start event');
    dragStartTaskId = e.currentTarget.dataset.taskId;
    e.dataTransfer.effectAllowed = 'move';
    e.dataTransfer.setData('text/plain', '');
}

function handleDragOver(e) {
    console.log('Drag start event');
    e.preventDefault();
    e.dataTransfer.dropEffect = 'move';
}

function handleDrop(e) {
    console.log('Drag start event');
    e.preventDefault();
    const dropTaskId = e.currentTarget.dataset.taskId;
    if (dragStartTaskId !== dropTaskId) {
        console.log(`Move task ${dragStartTaskId} to ${dropTaskId}`);
    }
}

cards.forEach(card => {
    console.log('Drag start event');
    card.addEventListener('dragstart', handleDragStart);
    card.addEventListener('dragover', handleDragOver);
    card.addEventListener('drop', handleDrop);
});
