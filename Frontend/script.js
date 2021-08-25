const svgStates = document.querySelectorAll("path");
const ctaButton = document.getElementById('button--cta');
const chromaticNumberSpan = document.getElementById('chromatic-number');
const stateSpan = document.getElementById('state-span');
const statesCountSpan = document.getElementById('states-count');
const selectAllButton = document.getElementById('select-all');

const selectedStates = [];

svgStates.forEach(s => {
    s.addEventListener('click',
        () => {
            s.style.fill = '';
            s.classList.toggle('active');
            if (selectedStates.includes(s.id)) {
                selectedStates.splice(selectedStates.indexOf(s.id), 1);
            } else {
                selectedStates.push(s.id);
            }

            statesCountSpan.innerText = selectedStates.length;
        });
    s.addEventListener('mousemove',
        event => {
            const posX = event.clientX;
            const posY = event.clientY;

            stateSpan.style.left = posX + 5;
            stateSpan.style.top = posY - 15;
            stateSpan.innerText = s.getAttribute('title');
        });
    s.addEventListener('mouseleave', () => {
        stateSpan.style.left = 0;
        stateSpan.style.top = 0;
        stateSpan.innerText = null;
    });
});

ctaButton.addEventListener('click', () => {
    const matrix = generateAdjacencyMatrix();

    const request = new XMLHttpRequest();
    request.open("POST", "https://localhost:44323/api/graph/color");
    request.setRequestHeader("Content-Type", "application/json");

    request.onload = (event) => {
        const coloringInfo = JSON.parse(event.target.response);
        processColoringInfo(coloringInfo);
    }

    request.send(JSON.stringify({ matrix: matrix }));
});

selectAllButton.addEventListener('click', function () {
    svgStates.forEach(s => {
        if (!selectedStates.includes(s.id)) {
            selectedStates.push(s.id);
            s.classList.add('active');
        }
    });

    statesCountSpan.innerText = selectedStates.length;
});

function generateAdjacencyMatrix() {
    const depth = selectedStates.length;
    const adjacencyMatrix = new Array(depth);
    for (let i = 0; i < depth; i++) {
        adjacencyMatrix[i] = new Array(depth);
    }

    for (let i = 0; i < depth; i++) {
        const adjacentStates = Array.from(svgStates)
            .filter(s => s.getAttribute('id') === selectedStates[i])[0]
            .getAttribute('adjacent-states')
            .split(' ')
            .filter(state => selectedStates.includes(state));

        for (let j = 0; j < depth; j++) {
            adjacencyMatrix[i][j] = false;
        }

        adjacentStates.forEach(state => {
            adjacencyMatrix[i][selectedStates.indexOf(state)] = true;
        });
    }

    return adjacencyMatrix;
}

function processColoringInfo(coloringInfo) {
    chromaticNumberSpan.innerText = coloringInfo.chromaticNumber;
    
    if (!coloringInfo.verticesColors) return;

    coloringInfo.verticesColors.forEach((color, index) => {
        const stateElement = Array.from(svgStates)
            .filter(svgState => svgState.getAttribute('id') === selectedStates[index])[0];

        stateElement.style.fill = switchColor(color);
    });
}

function switchColor(colorId) {
    switch (colorId) {
        case 0:
            return "red";
        case 1:
            return "green";
        case 2:
            return "blue";
        case 3:
            return "orange";
        case 4:
            return "violet";
        case 5:
            return "cyan";
        case 6:
            return "magenta";
    }
}