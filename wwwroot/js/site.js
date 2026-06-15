// ============================================================
// İhtiyaç Molası – site.js
// ============================================================

document.addEventListener('DOMContentLoaded', () => {

  // ── Navbar scroll efekti ──────────────────────────────────
  const nav = document.getElementById('mainNav');
  if (nav) {
    const onScroll = () => {
      nav.classList.toggle('scrolled', window.scrollY > 30);
    };
    window.addEventListener('scroll', onScroll, { passive: true });
    onScroll();
  }

  // ── Mobile menu toggle ────────────────────────────────────
  const toggle   = document.getElementById('navToggle');
  const mobileMenu = document.getElementById('navMobile');
  if (toggle && mobileMenu) {
    toggle.addEventListener('click', () => {
      const open = mobileMenu.classList.toggle('open');
      nav && nav.classList.toggle('mobile-open', open);
      toggle.setAttribute('aria-expanded', open);
    });
  }

  // ── Toast auto-dismiss (5 saniye) ─────────────────────────
  document.querySelectorAll('.toast').forEach(t => {
    setTimeout(() => {
      t.style.transition = 'opacity .4s ease, transform .4s ease';
      t.style.opacity = '0';
      t.style.transform = 'translateX(20px)';
      setTimeout(() => t.remove(), 400);
    }, 5000);
  });

  // ── Aktif nav link vurgula ───────────────────────────────
  const path = window.location.pathname.toLowerCase();
  document.querySelectorAll('.nav-link').forEach(link => {
    const href = link.getAttribute('href') || '';
    if (href !== '/' && path.startsWith(href.toLowerCase())) {
      link.classList.add('active');
    } else if (href === '/' && path === '/') {
      link.classList.add('active');
    }
  });

  // ── Smooth section anchor scroll ──────────────────────────
  document.querySelectorAll('a[href^="#"]').forEach(a => {
    a.addEventListener('click', e => {
      const target = document.querySelector(a.getAttribute('href'));
      if (target) {
        e.preventDefault();
        target.scrollIntoView({ behavior: 'smooth', block: 'start' });
      }
    });
  });

  // ── Card entrance animation (Intersection Observer) ───────
  if ('IntersectionObserver' in window) {
    const cards = document.querySelectorAll('.request-card, .stat-card, .detail-card, .side-section');
    const obs = new IntersectionObserver((entries) => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          entry.target.style.opacity = '1';
          entry.target.style.transform = 'translateY(0)';
          obs.unobserve(entry.target);
        }
      });
    }, { threshold: 0.08 });

    cards.forEach(card => {
      card.style.opacity = '0';
      card.style.transform = 'translateY(18px)';
      card.style.transition = 'opacity .5s ease, transform .5s ease';
      obs.observe(card);
    });
  }

});
