﻿using AutoMapper;
using ProductManagement.Application.DTOs;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.Responses;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<ProductDTO>> GetProductByIdAsync(string id)
        {
            var response = new ServiceResponse<ProductDTO>();
            try
            {
                var productEntity = await _productRepository.GetProductByIdAsync(id);
                if (productEntity is null)
                {
                    response.Message = "Produto não encontrado na base de dados.";
                }
                else
                {
                    response.Data = new ProductDTO
                    {
                        Id = productEntity.Id,
                        Name = productEntity.Name,
                        Price = productEntity.Price,
                        Supplier = productEntity.Supplier,
                        Active = productEntity.Active,
                        RegisteredAt = productEntity.RegisteredAt,
                        ModifiedAt = productEntity.ModifiedAt
                    };
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = $"Ocorreu um erro ao buscar o produto na base de dados. Erro: {ex.Message}";
                response.Success = false;
            }

            return response;
        }

        public async Task<ServiceCollectionResponse<ProductDTO>> GetProductsAsync()
        {
            var response = new ServiceCollectionResponse<ProductDTO>();
            try
            {
                var products = await _productRepository.GetProductsAsync();
                response.Data = products.Select(x => new ProductDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Supplier = x.Supplier,
                    Active = x.Active,
                    RegisteredAt = x.RegisteredAt,
                    ModifiedAt = x.ModifiedAt
                });

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = $"Ocorreu um erro ao buscar a lista de produtos na base de dados. Erro: {ex.Message}";
                response.Success = false;
            }

            return response;
        }

        public async Task<GenericResponse> AddProductAsync(ProductDTO productDTO)
        {
            var response = new GenericResponse();
            try
            {
                var productEntity = _mapper.Map<ProductEntity>(productDTO);
                await _productRepository.AddProductAsync(productEntity);
                response.Message = "Produto cadastrado com sucesso.";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = $"Ocorreu um erro ao cadastrar o produto na base de dados. Erro: {ex.Message}";
                response.Success = false;
            }

            return response;
        }

        public async Task<GenericResponse> UpdateProductAsync(string id, ProductDTO productDTO)
        {
            var response = new GenericResponse();
            try
            {
                var productEntity = _mapper.Map<ProductEntity>(productDTO);
                response.Success = await _productRepository.UpdateProductAsync(id, productEntity);
                response.Message = response.Success ?
                    "Produto atualizado com sucesso." :
                    "Não foi possível atualizar o produto pois o mesmo não foi encontrado na base de dados.";
            }
            catch (Exception ex)
            {
                response.Message = $"Ocorreu um erro ao atualizar o produto na base de dados. Erro: {ex.Message}";
                response.Success = false;
            }

            return response;
        }

        public async Task<GenericResponse> RemoveProductByIdAsync(string id)
        {
            var response = new GenericResponse();
            try
            {
                response.Success = await _productRepository.RemoveProductByIdAsync(id);
                response.Message = response.Success ?
                    "Produto removido com sucesso." :
                    "Não foi possível remover o produto pois o mesmo não foi encontrado na base de dados.";
            }
            catch (Exception ex)
            {
                response.Message = $"Ocorreu um erro ao remover o produto da base de dados. Erro: {ex.Message}";
                response.Success = false;
            }

            return response;
        }
    }
}
