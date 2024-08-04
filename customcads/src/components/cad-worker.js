self.onmessage = (e) => fetch(e.data.url)
    .then(response => response.arrayBuffer())
    .then(arrayBuffer => self.postMessage({ arrayBuffer }, [arrayBuffer]))
    .catch(error => self.postMessage({ error: error.message }));