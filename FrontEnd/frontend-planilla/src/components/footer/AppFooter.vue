<template>
    <footer class="App-footer">
      <div class="footer-content">
        <div class="footer-section">
          <h3>Vorlagenersteller</h3>
          <p>Simplificando la administración de planillas para pequeñas y medianas empresas.</p>
          <div class="social-icons">
            <a href="#"><i class="fab fa-facebook"></i></a>
            <a href="#"><i class="fab fa-twitter"></i></a>
            <a href="#"><i class="fab fa-instagram"></i></a>
            <a href="#"><i class="fab fa-linkedin"></i></a>
          </div>
        </div>
  
        <div class="footer-section">
          <h4>Producto</h4>
          <ul>
            <li><a href="#">Novedades</a></li>
          </ul>
        </div>
  
        <div class="footer-section">
          <h4>Recursos</h4>
          <ul>
            <li><a href="#">Documentación</a></li>
            <li><a href="#">Guías</a></li>
          </ul>
        </div>
  
        <div class="footer-section">
          <h4>Empresa</h4>
          <ul>
            <li><a href="#">Sobre nosotros</a></li>
            <li><a href="#">Contacto</a></li>
            <li><a href="#">Política de privacidad</a></li>
          </ul>
        </div>
      </div>
  
      <div class="footer-bottom">
        <hr />
        <p>© 2025 Vorlagenersteller. Todos los derechos reservados.</p>
      </div>
    </footer>
  </template>
  
  <script setup>
  </script>
  
  <style scoped>
  .footer {
    background-color: white;
  padding: 2rem 1rem 1rem;
  font-size: 0.9rem;
  border-top: 1px solid #e5e7eb;
  }
  
  .footer-content {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
    gap: 2rem;
  }
  
  .footer-section {
    flex: 1 1 200px;
  }
  
  .footer-section h3,
  .footer-section h4 {
    font-weight: bold;
    margin-bottom: 0.8rem;
  }
  
  .footer-section p {
    font-size: 0.9rem;
    margin-bottom: 1rem;
    color: #333;
  }
  
  .footer-section ul {
    list-style: none;
    padding: 0;
  }
  
  .footer-section li {
    margin-bottom: 0.5rem;
  }
  
  .footer-section a {
    color: #1a1a1a;
    text-decoration: none;
    font-size: 0.9rem;
  }
  
  .footer-section a:hover {
    text-decoration: underline;
  }
  
  .social-icons a {
    margin-right: 0.8rem;
    font-size: 1.1rem;
    color: #000;
  }
  
  .footer-bottom {
    text-align: center;
    margin-top: 2rem;
    font-size: 0.85rem;
    color: #555;
  }
  
  .footer-bottom hr {
    margin-bottom: 1rem;
    border: none;
    border-top: 1px solid #ddd;
  }
  </style>
  