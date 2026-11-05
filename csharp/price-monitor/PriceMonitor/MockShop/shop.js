// Fills in the product page from the shop's API, the way many stores do:
// the HTML has no price until this script runs.
(async () => {
  const id = location.pathname.split('/').pop();
  const response = await fetch(`/api/products/${id}`);
  const product = await response.json();

  document.title = `${product.name} | Kickstand Sneakers`;
  document.getElementById('photo').src = `/${id}.svg`;
  document.getElementById('photo').alt = product.name;
  document.getElementById('name').textContent = product.name;
  document.getElementById('price').textContent =
    `$${product.price.toFixed(2)}`;
  document.getElementById('stock').textContent =
    product.inStock ? 'In stock' : 'Sold out';
  document.getElementById('stock').classList.toggle('out', !product.inStock);
  document.getElementById('add-to-cart').disabled = !product.inStock;
})();
