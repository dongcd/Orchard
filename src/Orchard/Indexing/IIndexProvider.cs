﻿using System;
using System.Collections.Generic;

namespace Orchard.Indexing {
    public interface IIndexProvider : IDependency {
        /// <summary>
        /// Creates a new index
        /// </summary>
        void CreateIndex(string name);

        /// <summary>
        /// Checks whether an index is already existing or not
        /// </summary>
        bool Exists(string name);

        /// <summary>
        /// Deletes an existing index
        /// </summary>
        void DeleteIndex(string name);

        /// <summary>
        /// Whether an index is empty or not
        /// </summary>
        bool IsEmpty(string indexName);

        /// <summary>
        /// Gets the number of indexed documents
        /// </summary>
        int NumDocs(string indexName);

        /// <summary>
        /// Creates an empty document
        /// </summary>
        /// <returns></returns>
        IIndexDocument New(int documentId);

        /// <summary>
        /// Adds a new document to the index
        /// </summary>
        void Store(string indexName, IIndexDocument indexDocument);

        /// <summary>
        /// Adds a set of new document to the index
        /// </summary>
        void Store(string indexName, IEnumerable<IIndexDocument> indexDocuments);

        /// <summary>
        /// Removes an existing document from the index
        /// </summary>
        void Delete(string indexName, int documentId);

        /// <summary>
        /// Removes a set of existing document from the index
        /// </summary>
        void Delete(string indexName, IEnumerable<int> documentIds);

        /// <summary>
        /// Creates a search builder for this provider
        /// </summary>
        /// <returns>A search builder instance</returns>
        ISearchBuilder CreateSearchBuilder(string indexName);

        /// <summary>
        /// Returns the date and time when the index was last processed
        /// </summary>
        DateTime GetLastIndexUtc(string indexName);

        /// <summary>
        /// Sets the date and time when the index was last processed
        /// </summary>
        void SetLastIndexUtc(string indexName, DateTime lastIndexUtc);

    }
}