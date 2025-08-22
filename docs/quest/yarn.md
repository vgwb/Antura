# Test Yarn Script

<div id="yarn-graph"></div>


<script>
document.addEventListener('DOMContentLoaded', function() {
    YarnGraphRenderer.renderFromFile('yarn-graph', '../yarn/test.yarn', {
        height: 800,
        enableControls: true
    })
    .then(() => console.log('Yarn script loaded'))
    .catch(error => console.error('Yarn script FAILED to load:', error));
});
</script>
