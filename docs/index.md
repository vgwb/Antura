<script>
  // Redirect to English if on root (client-only guard for SSR)
  if (typeof window !== 'undefined' && (window.location.pathname === '/' || window.location.pathname === '/index.html')) {
    window.location.replace('/en/');
  }
</script>

<noscript>
  <meta http-equiv="refresh" content="0; url=/en/" />
  <p><a href="/en/">Continue to English content</a></p>
</noscript>
