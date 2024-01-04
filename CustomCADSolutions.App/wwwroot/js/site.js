/*
document.getElementById('imageForm').addEventListener('submit', function (e) {
    e.preventDefault();

    var description = document.getElementById('description').value;

    fetch('https://localhost:7182/DeepAI/GenerateImage', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ description: description })
    })
        .then(response => response.json())
        .then(data => {
            // Handle the response from your backend
            // For example, if your backend returns the URL of the generated image: 
            if (data.imageUrl) {
                document.getElementById('result').innerHTML =
                    <img src=' + data.imageUrl + ' alt="Generated Image" />;
            }
            else {
                console.log("No image URL returned");
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
});
*/