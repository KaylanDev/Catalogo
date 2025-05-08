# Documentação do Curso de API

Este repositório contém a documentação e os projetos desenvolvidos durante o curso de API, servindo como um registro completo de todo o conhecimento adquirido durante o período de aprendizado.

## Sobre o Projeto

Este projeto foi criado com o objetivo de documentar e organizar todo o conteúdo e práticas realizadas durante o curso de API. Aqui você encontrará:

- Exemplos de código
- Exercícios práticos
- Conceitos aprendidos
- Melhores práticas
- Documentação de endpoints
- E muito mais!

## Estrutura do Projeto

O projeto está organizado de forma a facilitar o acesso e compreensão dos diferentes tópicos abordados no curso. Cada seção representa um módulo ou conceito específico aprendido durante o período de estudo.

# Cache
A interface usada é IMemoryCache, com ela sera feita a implementação do Cache no projeto, podendo ser utilizado diretamento no controller ou como service

## no Controlador
no controlador, é usado menos codigo e menos complexidade, porem n é reutilizavel. exemplo

## Service
como um service, ha mais complexidade e mais codigo, porem é reutilizavel, recomendado para projetos medios e grandes

## SizeLimit e SetSize
ao definir um SizeLimit no Middleware, é obrigatirio definir um Size ao coonfigurar o cache, caso contrario n ira ser salvo em cache.

## implementacao controller

            if (!_memoryCache.TryGetValue(CacheProdutosKey,out IEnumerable<Produtos>? produtos))
            {
                produtos = await _uof.ProductRepository.GetAllAsync();

                if (produtos is not null && produtos.Any())
                {
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                        SlidingExpiration = TimeSpan.FromSeconds(15),
                        Priority = CacheItemPriority.High,
                    };

                    _memoryCache.Set(CacheProdutosKey, produtos, cacheOptions);

                }
                else
                {
                    return NotFound("Nenhum produto encontrado");
                }

            }
